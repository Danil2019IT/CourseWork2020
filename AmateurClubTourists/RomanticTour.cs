using System;

namespace AmateurClubTourists
{
    [Serializable]
    public class RomanticTour : Tour
    {
        // RomanticTour class Defines the characteristics of a
        // particular type of tour: romantic tour.
        
        public RomanticTour(){} // default constructor

        public RomanticTour(RomanticTour romanticTour): // copy constructor
            this(
                romanticTour.Name,
                romanticTour.Duration,
                romanticTour.Price,
                romanticTour.Location,
                romanticTour.Description,
                romanticTour.TourStart
                ){}
        
        public RomanticTour(
            string name,
            int duration,
            decimal price,
            string location,
            string description,
            DateTime tourStart 
            ) : base(
            name,
            duration,
            price,
            location,
            TypesOfTours.RomanticTour,
            description,
            tourStart
        ){}
    }
}