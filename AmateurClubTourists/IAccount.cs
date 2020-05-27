namespace AmateurClubTourists
{
    public interface IAccount
    {
        // Basic characteristics of a bank account
        void Deposit(decimal amount);
        void Withdraw(decimal amount);
        decimal Balance { get; }
        void Show();
    }
}