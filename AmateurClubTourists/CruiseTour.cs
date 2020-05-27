using System;

namespace AmateurClubTourists
{
    [Serializable]
    public class CruiseTour : Tour
    {
        // CruiseTour class Defines the characteristics of a
        // particular type of tour: cruise tour.
        
        public CruiseTour(){} // default constructor
        
        public CruiseTour(CruiseTour cruiseTour): // copy constructor
            this(
                cruiseTour.Name,
                cruiseTour.Duration,
                cruiseTour.Price,
                cruiseTour.Location,
                cruiseTour.MaxGroupSize,
                cruiseTour.Description,
                cruiseTour.TourStart
            ){}
        
        public CruiseTour(
            string name,
            int duration,
            decimal price,
            string location,
            int maxGroupSize,
            string description,
            DateTime tourStart
            ) : base(
            name,
            duration,
            price,
            location,
            TypesOfTours.CruiseTour, 
            description,
            tourStart
            )
        {
            if (maxGroupSize <= 0) throw new ArgumentException(nameof(maxGroupSize)); // check validity of parameters
            MaxGroupSize = maxGroupSize;
        }
        
        public int MaxGroupSize { get; set; }
        
        public override string ToString()
        {
            return $"{base.ToString()}\n" +
                   $"Max group size: {MaxGroupSize}\n";
        }
    }
}