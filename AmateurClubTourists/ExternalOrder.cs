using System;

namespace AmateurClubTourists
{
    public class ExternalOrder : Order
    {
        // ExternalOrder class determines the order
        // that relates to the agency’s essence orders.
        
        public ExternalOrder(){} // default constructor
        
        public ExternalOrder(ExternalOrder externalOrder): // copy constructor
            this(
                externalOrder.Name,
                externalOrder.RegistrationDate,
                externalOrder.Price,
                externalOrder.Agency
            ){}
        public ExternalOrder(string name, DateTime date, decimal price, Agency agency)
        : base(name, date, price)
        {
            Agency = agency ?? throw new ArgumentNullException(nameof(agency)); // check null
        }
        public Agency Agency { get; set; }

        public override string PrintInfo()
        {
            return $"{base.PrintInfo()}Agency: {Agency.Name}\n";
        }
        
        public override void Clear()
        {
            base.Clear();
            Agency = null;
        }
    }
}