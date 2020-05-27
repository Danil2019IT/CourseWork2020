using System;
using Microsoft.VisualBasic;

namespace AmateurClubTourists
{
    [Serializable]
    public class SightseeingTour : Tour
    {
        // SightseeingTour class Defines the characteristics of a
        // particular type of tour: sightseeing tour.
        
        public SightseeingTour(){} // default constructor
        
        public SightseeingTour(SightseeingTour sightseeingTour): // copy constructor
            this(
                sightseeingTour.Name,
                sightseeingTour.Duration,
                sightseeingTour.Price,
                sightseeingTour.Location,
                sightseeingTour.WayToTravel,
                sightseeingTour.Description,
                sightseeingTour.TourStart
                ){}
        
        public SightseeingTour(
            string name,
            int duration,
            decimal price,
            string location,
            string wayToTravel,
            string description,
            DateTime tourStart
        ) : base(
            name,
            duration,
            price,
            location,
            TypesOfTours.SightseeingTour,
            description,
            tourStart
            )
        {
            WayToTravel = wayToTravel ?? throw new ArgumentNullException(nameof(wayToTravel)); // check null
        }
        public string  WayToTravel { get; set; }

        public override string ToString()
        {

            return $"{base.ToString()}\n" +
                   $"WayToTravel: {WayToTravel}\n";
        }
        
    }
}