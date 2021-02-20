using Entities.Models;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Task<Account> GetAccountById(int accountId);
        Task<Account> GetAccountWithDetails(int accountId);
    }
}