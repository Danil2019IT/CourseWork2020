using System;

namespace AmateurClubTourists
{
    public class Order : IPricing
    {
        // Order class defines the basic order fields
        
        public Order(){} // default constructor
        
        public Order(Order order): // copy constructor
            this(
                order.Name,
                order.RegistrationDate,
                order.Price
                ){}
        public Order(string name, DateTime date, decimal price)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name)); // check null
            RegistrationDate = date;
            
            if (price <= 0) throw new ArgumentException(nameof(price)); // check validity of parameters
            Price = price;
        }

        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal Price { get; set; }

        public virtual string PrintInfo()
        {
            return $"Name order: {Name}\n" +
                              $"Price: {Price}\n" +
                              $"Date registration order: {RegistrationDate}\n";
        }

        public virtual void Clear()
        {
            Name = "";
            Price = 0;
            RegistrationDate = new DateTime();
        }
    }
}