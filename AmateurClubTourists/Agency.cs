using System;
using System.Collections.Generic;
using System.Linq;

namespace AmateurClubTourists
{
    [Serializable]
    public class Agency : IDiscountManager
    {
        // Agency class defines the basic characteristics
        // and behaviour of the agency that provides tour sales services.
        
        public delegate void Status(object sender, string message);
        public event Status Notify;

        public Agency(Agency agency): // copy constructor
            this(
                agency.Name,
                agency.Address,
                agency.PhoneNumber,
                agency.Email,
                agency._tours,
                agency.About,
                agency.Rating
                ){}
        
        public Agency(
            string name,
            string address,
            string phoneNumber,
            string email,
            List<Tour> tours,
            string about,
            double rating = 0
            )
        {
            Name = name ?? throw new ArgumentNullException(nameof(name)); // check null
            Address = address ?? throw new ArgumentNullException(nameof(address)); // check null
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber)); // check null
            Email = email ?? throw new ArgumentNullException(nameof(email)); // check null
            _tours = tours;
            About = about ?? throw new ArgumentNullException(nameof(about)); // check null
            if (rating < 0 || rating > 5) throw new ArgumentOutOfRangeException(nameof(rating)); // check validity of parameters
                Rating = rating;
        }
        
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public double Rating { get; private set; }
        public string About { get; private set; }

        private readonly List<Tour> _tours;
        private readonly List<User> _users = new List<User>();
        

        public List<Tour> ListToursByType(TypesOfTours type)
        {
            /*  
             *    input data: type - specific type of tour
             *    return data: list of tours that match this type
             */
            return _tours.Where(t => t.Type == type).ToList();
        }
        
        public List<Tour> ListToursByDate(DateTime dateTime)
        {
            /*  
             *    input data: dateTime - specific date
             *    return data: list of tours that match this date
             */
            return _tours.Where(t => t.TourStart <= dateTime && dateTime <= t.TourEnd).ToList();
        }
        
        public List<Tour> ListToursByPrice(decimal startPrice, decimal endPrice)
        {
            /*  
             *    input data: dateTime - price range
             *    return data: list of tours that match this price range
             */
            return _tours.Where(t => startPrice <= t.Price && t.Price <= endPrice).ToList();
        }
        
        public List<Tour> ListToursByCountry(string country)
        {
            /*  
             *    input data: country - specific country
             *    return data: list of tours that match this country
             */
            return _tours.Where(t => t.Location.ToLower().Contains(country.ToLower())).ToList();
        }
        
        public List<Tour> ListToursByDescription(string marker)
        {
            /*  
             *    input data: marker - specific word-marker
             *    return data: list of tours all matches with a marker word
             */
            return _tours.Where(t => t.Description.ToLower().Contains(marker.ToLower())).ToList();
        }
        
        public List<Tour> ListAllTours()
        {
            return _tours;
        }
        
        public int ToursCountByType(TypesOfTours type)
        {
            return _tours.Count(tour => tour.Type == type);
        }

        public int CountTours()
        {
            return _tours.Count;
        }

        public void AddTours(Tour tour)
        {
            if (tour == null) throw new ArgumentNullException(nameof(tour));
            
            _tours.Add(tour);
        }

        public void AddUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            _users.Add(user);
            Notify?.Invoke(this, $"Go on vacation with {Name}\n");
        }

        public void RemoveUser(User user)
        {
            _users.Remove(user);
            Notify?.Invoke(this, $"{Name} will always be glad to see you :)\n");
        }

        private bool IsPossible(User user, decimal cost)
        {
            // check for payment
            return user.Account.Balance >= cost;
        }
        
        public bool PayOrder(User user, ExternalOrder order)
        {
            // method that pays for a tour that is already booked
            
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (order == null) throw new ArgumentNullException(nameof(order));

            if (!IsPossible(user, order.Price))
            {
                Notify?.Invoke(this, $"Not enough money? :( We will have something that suits you ;)\n");
                return false;
            }
            user.Account.Withdraw(order.Price);
            Notify?.Invoke(this, $"Have a good rest :) Thank you for choosing us!\n");
            return true;
        }

        public ExternalOrder AddToOrder(User user, Tour tour)
        {
            // method for booking a tour
            
            var newPrice = ApplyDiscount(tour.Price, user.UserStatus, user.TimeOfHavingAccountInYears);
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (tour == null) throw new ArgumentNullException(nameof(tour));
            Notify?.Invoke(this, $"Price including all discounts is {newPrice}$\n");
            return new ExternalOrder(tour.Name, DateTime.Now, newPrice, this);
        }

        public override string ToString()
        {
            return " Contact details\n" +
                   $" Name: \"{Name}\"\n" +
                   $" Address: {Address}\n" +
                   $" PhoneNumber: {PhoneNumber}\n" +
                   $" Email: {Email}\n";
        }

        public decimal ApplyDiscount(decimal price, UserStatus accountStatus, int timeOfHavingAccountInYears)
        {
            // method for calculating the cost of the tour,
            // taking into account all possible discounts
            /*
             * input data:
             *     price - cost before discount;
             *     accountStatus - current customer status;
             *     timeOfHavingAccountInYears - number of years user account exists;
             * 
             * return data: cost after discount;
             */
            decimal priceAfterDiscount = 0;
                var discountForLoyaltyInPercentage = timeOfHavingAccountInYears > 5 ? (decimal)5/100 : (decimal)timeOfHavingAccountInYears/100;
                
                switch (accountStatus)
                {
                    case UserStatus.SimpleCustomer:
                        priceAfterDiscount = price - discountForLoyaltyInPercentage *  0.1m * price;
                        break;
                    case UserStatus.ValuableCustomer:
                        priceAfterDiscount = 0.85m * price - discountForLoyaltyInPercentage * 0.85m * price;
                        break;
                    case UserStatus.MostValuableCustomer:
                        priceAfterDiscount = 0.75m * price - discountForLoyaltyInPercentage * 0.75m * price;
                        break;
                }
                return priceAfterDiscount;
        }
    }
}