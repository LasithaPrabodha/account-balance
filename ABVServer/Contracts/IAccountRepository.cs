using Entities.Models;

namespace Contracts
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Account GetAccountById(int accountId);
        Account GetAccountWithDetails(int accountId);
    }
}