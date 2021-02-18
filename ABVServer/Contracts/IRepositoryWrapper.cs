namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IAccountRepository Account { get; }
        ITransactionRepository Transaction { get; }
        void Save();
    }
}