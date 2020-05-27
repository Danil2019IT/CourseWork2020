using System;
using System.Globalization;

namespace AmateurClubTourists
{
    [Serializable]
    public abstract class Tour : IPricing
    {
        // Tour class defining the main fields that
        // belong to the nature of the tour.
        
        protected Tour(){} // default constructor
        protected Tour(
            string name,
            int duration,
            decimal price,
            string location,
            TypesOfTours typesOfTours,
            string description,
            DateTime tourStart
        )
        {
            Name = name ?? throw new ArgumentNullException(nameof(name)); // check null
            if (duration <= 0) throw new ArgumentException(nameof(duration)); // check validity of parameters
            Duration = duration;
            if (price < 0) throw new ArgumentException(nameof(duration)); // check validity of parameters
            Price = price;
            Location = location ?? throw new ArgumentNullException(nameof(location)); // check null
            Type = typesOfTours;
            Description = description ?? throw new ArgumentNullException(nameof(description)); // check null
            TourStart = tourStart;
            TourEnd = TourStart.AddDays(Duration);
        }
        public string Name { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public TypesOfTours Type { get; }
        public string Location { get; set; }
        public string Description { get; set; }
        
        public DateTime TourStart { get; set; }
        
        public DateTime TourEnd { get; set; }
        public override string ToString()
        {
            return  "Details\n" +
                   $"Name: \"{Name}\"\n" +
                   $"Duration: {Duration} days\n" +
                   $"Price: {Price}$\n" +
                   $"Location: {Location}\n\n" +
                   " Description: \n" +
                   $"{Description}\n" +
                   $"\nDates: {TourStart.ToString("D", CultureInfo.CreateSpecificCulture("en-US"))} -- {TourEnd.ToString("D",CultureInfo.CreateSpecificCulture("en-US"))}\n" +
                   $"Start time -> {TourStart.ToShortTimeString()}\n";
        }

        
    }
}