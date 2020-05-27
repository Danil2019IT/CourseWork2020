namespace AmateurClubTourists
{
    public interface IStatement
    {
        // Simple account status support interface
        int Transactions { get; }
        void Show();
    }
}