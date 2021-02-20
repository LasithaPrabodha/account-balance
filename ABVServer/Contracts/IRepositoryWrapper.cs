using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IAccountRepository Account { get; }
        ITransactionRepository Transaction { get; }
        IUserRepository User { get; }
        Task Save();
    }
}