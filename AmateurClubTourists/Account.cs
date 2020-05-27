using System;

namespace AmateurClubTourists
{
    [Serializable]
    public class Account : IAccount, IStatement
    {
        // Account class defines the behavior of the user’s bank account
        
        public delegate void Status(object sender, string message);
        public event Status Notify;

        public Account(decimal balance)
        {
            Balance = balance;
        }

        public void Deposit(decimal amount)
        {
            if (amount < 200) throw new ArgumentException(nameof(amount));
            Balance += amount;
            ++Transactions;
            Notify?.Invoke(this, $"Accrued deposit in the amount of {amount}$\n" +
                                                    $"Current balance: {Balance}\n");
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException(nameof(amount));
            Balance -= amount;
            Notify?.Invoke(this, $"Amount withdrawn: {amount}$\n" +
                                                    $"Current balance: {Balance}\n");
            ++Transactions;
        }

        public decimal Balance { get; private set; }

        void IAccount.Show()
        {
            Console.WriteLine($"balance = {Balance}");
        }

        public int Transactions { get; private set; }

        void IStatement.Show()
        {
            Console.WriteLine($"{Transactions} transactions, balance = {Balance}");
        }
    }
}