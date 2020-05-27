using System;

namespace AmateurClubTourists
{
    [Serializable]
    public class AdventureTour : Tour
    {
        // AdventureTour class Defines the characteristics of a
        // particular type of tour: adventure tour.
        
        public AdventureTour(){} // default constructor
        
        public AdventureTour(AdventureTour adventureTour):  // copy constructor
            this(
                adventureTour.Name,
                adventureTour.Duration,
                adventureTour.Price,
                adventureTour.Location,
                adventureTour.PhysicalRating,
                adventureTour.Description,
                adventureTour.TourStart
            ){}
    
        public AdventureTour(
            string name,
            int duration,
            decimal price,
            string location,
            string physicalRating,
            string description,
            DateTime tourStart 
            ) : base(
            name,
            duration,
            price,
            location,
            TypesOfTours.AdventureTour,
            description,
            tourStart
            )
        {
            PhysicalRating = physicalRating;
        }
        
        public string PhysicalRating { get; set; }
        public override string ToString()
        {
            return $"{base.ToString()}\n" +
                   $"Physical rating: {PhysicalRating}\n";
        }
    }
}