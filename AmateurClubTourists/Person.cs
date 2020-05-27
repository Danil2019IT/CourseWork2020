using System;

namespace AmateurClubTourists
{
    [Serializable]
    public abstract class Person
    {
        // Person class defining the essence of a human with a basic set of characteristics
        
        protected Person(){} // default constructor
        protected Person(
            string firstName,
            string lastName,
            int age,
            string gender
        )
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName)); // check null
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName)); // check null
            if (age <= 0) throw new ArgumentException(nameof(age)); // check validity of parameters
            Age = age;
            if (gender != "Male" && gender != "Female") throw new ArgumentException(nameof(gender));
            Gender = gender;

        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Age { get; private set; }
        public string Gender { get; }
    }
}