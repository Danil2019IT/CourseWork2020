namespace AmateurClubTourists
{
    public interface IDiscountManager
    {
        public decimal ApplyDiscount(decimal price, UserStatus accountStatus, int timeOfHavingAccountInYears);
    }
}