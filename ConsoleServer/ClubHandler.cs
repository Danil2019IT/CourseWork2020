using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using AmateurClubTourists;

namespace ConsoleServer
{
    public static class ClubHandler
    {
        private static Club _club;
        private static User _user;

        private static void UpdateData()
        {
            var formatter = new BinaryFormatter();
            using var fs = new FileStream("club.dat", FileMode.OpenOrCreate);
            _club = (Club)formatter.Deserialize(fs);
        }

        private static byte[] Help()
        {
            return Encoding.Unicode.GetBytes("Available commands:\n" +
                                                "1) Select an agency\n" +
                                                "2) Meet the members of the club\n" +
                                                "3) Exit\n");
        }

        private static void NewUser(User user)
        { 
            _club.AddUser(user);
        }
        
        private static byte[] MembersOfTheClub()
        {
            var users = _club.ListUsers();
            var message = "";
            for (var index = 0; index < users.Count; index++)
            {
                message += $"{index+1}) {users[index].FirstName} {users[index].LastName} -> {users[index].Email}\n";
            }
            return Encoding.Unicode.GetBytes(message);
        }

        private static (byte[], List<Agency> agencies) AgenciesOfTheClub()
        {
            var agencies = _club.ListAgencies();
            var message = "";
            for (var index = 0; index < agencies.Count; index++)
            {
                message += $"{index+1}) {agencies[index].Name} -- {agencies[index].Rating}/5\n";
            }
            return (Encoding.Unicode.GetBytes(message), agencies);
        }

        private static void UserConfirmation(NetworkStream stream)
        {
            while (true)
            {
                // get message
                var message = BuilderMessage(stream);
                switch (message)
                {
                    case "1":
                        LogIn(stream);
                        if (_user == null) break;
                        return;

                    case "2":
                        Registration(stream);
                        return;
                }
                stream.Write(Encoding.Unicode.GetBytes("1) Log In\n" +
                                                       "2) Registration\n"));
            }
        }

        private static void Registration(NetworkStream stream)
        {
            while (true)
            {
                stream.Write(Encoding.Unicode.GetBytes("Enter your email: "));
                var email = BuilderMessage(stream);
                stream.Write(Encoding.Unicode.GetBytes("Enter your password: "));
                var password = BuilderMessage(stream);
                try
                {
                    _user = new User("User", "User", 20, "Male", email, 2000, password: password);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    _user = null;
                }

                if (_user != null)
                {
                    return;                    
                }
            }
        }
        
        private static void LogIn(NetworkStream stream)
        {
            var i = 3; 
            while (i > 0)
            {
                stream.Write(Encoding.Unicode.GetBytes("Enter your email: "));
                var email = BuilderMessage(stream);
                stream.Write(Encoding.Unicode.GetBytes("Enter your password: "));
                var password = BuilderMessage(stream);
                foreach (var user in _club.ListUsers().Where(user => user.Email == email && user.Password == password))
                {
                    _user = user;
                    return;
                }
                i--;
                stream.Write(Encoding.Unicode.GetBytes("Incorrect data :( \n"));
            }
            stream.Write(Encoding.Unicode.GetBytes("Restore password:\n" +
                                                   "1) Yes\n" +
                                                   "2) No\n"));
            var message = BuilderMessage(stream);
            if (message == "1") RestorePassword(stream);
            else stream.Write(Encoding.Unicode.GetBytes("If you don’t have an account, you can always create a new one or reset your password\n"));
        }

        private static void RestorePassword(NetworkStream stream)
        {
            var i = 3;
            while (i > 0)
            {
                stream.Write(Encoding.Unicode.GetBytes("Enter your email: "));
                var email = BuilderMessage(stream);
                foreach (var user in _club.ListUsers().Where(user => user.Email == email))
                {
                    stream.Write(Encoding.Unicode.GetBytes("Enter new password: "));
                    var password = BuilderMessage(stream);
                    _user = user;
                    _user.Password = password;
                    return;
                }

                i--;
                stream.Write(Encoding.Unicode.GetBytes("User with this email does not exist. :( \n"));
            }
        }

        private static string BuilderMessage(NetworkStream stream)
        {
            var data = new byte[128];
            var builder = new StringBuilder();
            do
            {
                var bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            var message = builder.ToString();
            return message;
        }
        
        public static void ClubInteraction(NetworkStream stream)  
        {
            stream.Write(Encoding.Unicode.GetBytes("......................... ,------,\n" +
                                                        "......................... =\\ .....\\\n"+
                                                        "..,---,................... =\\ .....\\\n" +
                                                        "..|.C~ \\ .................. =\\ .....\\\n" +
                                                        "..|....... `------------------'------'-----,\n" +
                                                        ".,'.... LI,-,LI. LI. LI . LI .. LI . LI ,-,LI `-,\n" +
                                                        ".\\ _/,____|_|_______________,------,________|_|____)\n" +
                                                        ".......................... /...... /\n" +
                                                        "........................ =/...... / \n" +
                                                        "....................... =/...... /\n" +
                                                        "...................... =/...... /\n" +
                                                        "...................... /______,'\n"));
            UpdateData();
            UserConfirmation(stream);
            _user.Account.Notify += (sender, message) => stream.Write(Encoding.Unicode.GetBytes(message));
            _club.Notify += (sender, message) => stream.Write(Encoding.Unicode.GetBytes(message));
            
            if (!_club.UserInClub(_user))
            {
                NewUser(_user);
            }
            
            stream.Write(Help());
            while (true)
            {
                // get message
                var message = BuilderMessage(stream);
                Console.WriteLine(message);
                if (message == "3")
                {
                    stream.Write(Encoding.Unicode.GetBytes("Thanks for your visit :)"));
                    stream.Close();
                    break;
                }

                byte[] data; // buffer for received data
                if (message != "1")
                {
                    data = message switch
                    {
                        "2" => MembersOfTheClub(),
                        _ => Help()
                    };
                    stream.Write(data, 0, data.Length);
                    continue;
                }

                var (item1, agencies) = AgenciesOfTheClub();
                data = item1;
                stream.Write(data);
                AgencyHandler.AgencyInteraction(_user, stream, agencies);
                stream.Write(Help());
            }
        }
    }
}