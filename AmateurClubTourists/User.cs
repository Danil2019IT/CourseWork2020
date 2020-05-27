using System;

namespace AmateurClubTourists
{
    [Serializable]
    public class User : Person
    {
        // Tourism club user-defining class with the necessary basic characteristics
        
        public User(){} // default constructor
        
        public User(User user): // copy constructor
            this(
                user.FirstName,
                user.LastName,
                user.Age,
                user.Gender,
                user.Email,
                user.Account.Balance,
                user.TimeOfHavingAccountInYears,
                user.UserStatus
                ){}
        
        public User(
            string firstName,
            string lastName,
            int age,
            string gender,
            string email,
            decimal balance,
            int timeOfHavingAccountInYears = 0,
            UserStatus userStatus = UserStatus.SimpleCustomer,
            string password = "default"
            ) : base(firstName, lastName, age,  gender)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email)); // check null
            
            if (balance < 0) throw new ArgumentException(nameof(balance));  // check validity of parameters
            
            Account = new Account(balance);
            InClub = true; 
            
            if (timeOfHavingAccountInYears < 0) throw new ArgumentException(nameof(timeOfHavingAccountInYears));  // check validity of parameters
            TimeOfHavingAccountInYears = timeOfHavingAccountInYears;
            
            UserStatus = userStatus;

            if (password == null || password.Length < 6) throw new ArgumentException(nameof(password));
            _password = password;
        }
        
        public string Email { get; set; }
        public Account Account { get; internal set; }
        public bool InClub { get; private set; }
        public int TimeOfHavingAccountInYears { get; private set; }
        public UserStatus UserStatus { get; internal set; }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                if (value == null || value.Length < 6) throw new ArgumentException(nameof(value));
                _password = value;
            }
        }
    }
}