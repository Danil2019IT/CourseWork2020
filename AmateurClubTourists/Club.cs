using System;
using System.Collections.Generic;

namespace AmateurClubTourists
{
    [Serializable]
    public class Club
    {
        // Club class determines the possibilities of the Club Lovers of Tourism
        
        public delegate void Welcome(object sender, string message);
        public event Welcome Notify;
        
        public Club(){} // default constructor

        public Club(Club club) : this(club.NameClub, club._users, club._agencies) // copy constructor
        {}
        
        public Club(string nameClub, List<User> users, List<Agency> agencies)
        {
            NameClub = nameClub ?? throw new ArgumentNullException(nameof(nameClub)); // check null
            _users = users;
            _agencies = agencies;
        }
        
        public string NameClub { get; private set; }
        private readonly List<User> _users;
        private readonly List<Agency> _agencies;

        public List<User> ListUsers()
        {
            return _users;
        }

        public bool UserInClub(User user)
        {
            return _users.Contains(user);
        }
        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            
            _users.Add(user);
            Notify?.Invoke(this,$"Welcome to {NameClub}\n");
        }
        public int CountUsers()
        {
            return _users.Count;
        }
        
        public List<Agency> ListAgencies()
        {
            return _agencies;
        }
        
        public void AddAgency(Agency agency)
        {
            if (agency == null)
                throw new ArgumentNullException(nameof(agency));
            
            _agencies.Add(agency);
        }

        public int CountAgencies()
        {
            return _agencies.Count;
        }

        public Agency GetAgency(int index)
        {
            if (index <= 0 || index > _agencies.Count)
                throw new IndexOutOfRangeException(nameof(index));
            
            return _agencies[index - 1];
        }
    }
}