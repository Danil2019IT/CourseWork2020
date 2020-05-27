using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AmateurClubTourists;

namespace ConsoleServer
{
    public static class AgencyHandler
    {
        private static Agency _agency;
        private static ExternalOrder _order;
        
        private static void UpdateData(Agency agency)
        {
            _agency = agency;
        }
        
        private static string Help()
        {
            return "1) Search by type \n" +
                   "2) Search by country\n" + 
                   "3) Marker search\n" +
                   "4) Search by date\n" + 
                   "5) Search by price\n" + 
                   "6) Change agents\n" +
                   "7) Contacts agents\n" +
                   "8) Booked Tours\n" +
                   "0) Exit\n";
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

        private static void StreamWriter(string message, NetworkStream stream)
        {
            var data = Encoding.Unicode.GetBytes(message);
            stream.Write(data);
        }
        
        private static (byte[], List<Tour>) ToursByType(NetworkStream stream)
        {
            StreamWriter("Select type:\n" +
                         "1) SightseeingTour\n" +
                         "2) AdventureTour\n" +
                         "3) CruiseTour\n" +
                         "4) RomanticTour", stream);
            var message = BuilderMessage(stream);
            if (Convert.ToInt16(message) < 1 || Convert.ToInt16(message) > 4) return (new byte[] { }, null); 
            
            var tours = _agency.ListToursByType((TypesOfTours)Convert.ToInt16(message));
            
            message = "";
            for (var index = 0; index < tours.Count; index++)
            {
                message += $"{index+1}) {tours[index].Name} -- {tours[index].Price}$\n";
            }
            return (Encoding.Unicode.GetBytes(message), tours);
        }
        
        private static (byte[], List<Tour>) ToursByCountry(NetworkStream stream)
        {
            StreamWriter("Input country", stream);
            var message = BuilderMessage(stream);
            var tours = _agency.ListToursByCountry(message);
            if (tours.Count == 0) return (new byte[] { }, null); 
            message = "";
            for (var index = 0; index < tours.Count; index++)
            {
                message += $"{index+1}) {tours[index].Name} -- {tours[index].Price}$\n";
            }
            return (Encoding.Unicode.GetBytes(message), tours);
        }
        
        private static (byte[], List<Tour>) ToursMarkerSearch(NetworkStream stream)
        {
            StreamWriter("Input word-marker", stream);
            var message = BuilderMessage(stream);
            var tours = _agency.ListToursByDescription(message);
            if (tours.Count == 0) return (new byte[] { }, null); 
            message = "";
            for (var index = 0; index < tours.Count; index++)
            {
                message += $"{index+1}) {tours[index].Name} -- {tours[index].Price}$\n";
            }
            return (Encoding.Unicode.GetBytes(message), tours);
        }
        
        private static (byte[], List<Tour>) ToursByDate(NetworkStream stream)
        {
            StreamWriter("Input date (dd/mm/yyyy)", stream);
            var message = BuilderMessage(stream);
            List<Tour> tours = null;
            try
            {
                tours = _agency.ListToursByDate(Convert.ToDateTime(message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (tours == null) return (new byte[] { }, null); 
            message = "";
            for (var index = 0; index < tours.Count; index++)
            {
                message += $"{index+1}) {tours[index].Name} -- {tours[index].Price}$\n";
            }
            return (Encoding.Unicode.GetBytes(message), tours);
        }
        
        private static (byte[], List<Tour>) ToursByPrice(NetworkStream stream)
        {
            StreamWriter("Min price:", stream);
            var message1 = BuilderMessage(stream);
            StreamWriter("Max price:", stream);
            var message2 = BuilderMessage(stream);
            List<Tour> tours = null;
            try
            {
                tours = _agency.ListToursByPrice(Convert.ToDecimal(message1), Convert.ToDecimal(message2));
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            if (tours == null || tours.Count == 0) return (new byte[] { }, null); 
            message1 = "";
            for (var index = 0; index < tours.Count; index++)
            {
                message1 += $"{index+1}) {tours[index].Name} -- {tours[index].Price}$\n";
            }
            return (Encoding.Unicode.GetBytes(message1), tours);
        }

        private static void TourHandler(User user, NetworkStream stream, (byte[], List<Tour>) tours)
        {
            if (tours.Item2 == null || tours.Item2.Count == 0)
            {
                StreamWriter("No matches found :(\n", stream);
                return;
            } 
            stream.Write(tours.Item1);
            var message = BuilderMessage(stream);
            var tour = tours.Item2[Convert.ToInt16(message)-1];
            StreamWriter(tour.ToString(), stream);
            StreamWriter("1) Book a tour\n" +
                         "2) Go back to choosing\n", stream);
            
            message = BuilderMessage(stream);

            if (message == "1")
            {
                _order = _agency.AddToOrder(user, tour);
            }
        }

        private static void PayHandler(User user, NetworkStream stream)
        {
            StreamWriter("1) Pay\n" +
                         "2) Drop\n", stream);

            var message = BuilderMessage(stream);

            if (message != "1") return;
            if (_agency.PayOrder(user, _order))
            {
                _agency.PayOrder(user, _order);
                return;
            }
            StreamWriter($"On your account: {user.Account.Balance}\n" +
                         $"Necessary: {_order.Price}\n", stream);
            
            StreamWriter("Would you like to make an interest-free deposit?\n" +
                         "1) Yes\n" +
                         "2) No\n", stream);

            message = BuilderMessage(stream);
            if (message != "1") return;
            user.Account.Deposit(_order.Price-user.Account.Balance);
            _agency.PayOrder(user, _order);
        }
        
        public static void AgencyInteraction(User user, NetworkStream stream, List<Agency> agencies)
        {
            var message = BuilderMessage(stream);
            if (Convert.ToInt16(message) < 0 || Convert.ToInt16(message) > agencies.Count) return;
            UpdateData(agencies[Convert.ToInt32(message)-1]);
            _agency.Notify += (sender, s) => stream.Write(Encoding.Unicode.GetBytes(s));
            _agency.AddUser(user);
            Thread.Sleep(10);
            StreamWriter( Help(), stream);
            while (true)
            {
                message = BuilderMessage(stream);
                (byte[], List<Tour>) tours;
                switch (message)
                {
                    case "1":
                        tours = ToursByType(stream);
                        TourHandler(user, stream, tours);
                        break;
                    case "2":
                        tours = ToursByCountry(stream);
                        TourHandler(user, stream, tours);
                        break;
                    case "3":
                        tours = ToursMarkerSearch(stream);
                        TourHandler(user, stream, tours);
                        break;
                    case "4":
                        tours = ToursByDate(stream);
                        TourHandler(user, stream, tours);
                        break;
                    case "5":
                        tours = ToursByPrice(stream);
                        TourHandler(user, stream, tours);
                        break;
                    case "6":
                        _agency.RemoveUser(user);
                        _order?.Clear();
                        return;
                    case "7":
                        StreamWriter(_agency.ToString(), stream);
                        break;
                    case "8":
                        StreamWriter(_order != null ? _order.PrintInfo()  : "It's empty here", stream);
                        break;
                    case "0":
                        StreamWriter("Have a nice day :)", stream);
                        _order?.Clear();
                        return;
                }
                if (message == "8" && _order != null)
                {
                    PayHandler(user, stream);
                    StreamWriter("Would you like to continue?\n" +
                                 "1) Yes\n" +
                                 "2) No\n" , stream);
                    if (BuilderMessage(stream) == "2")
                    {
                        return;
                    }
                }
                StreamWriter(Help(), stream);
            }
        }
    }
}