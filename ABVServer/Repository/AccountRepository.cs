using Contracts;
using Entities;
using Entities.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Account> GetAccountById(int accountId)
        {
            return await FindByCondition(a => a.Id.Equals(accountId))
                .FirstOrDefaultAsync();
        }

        public async Task<Account> GetAccountWithDetails(int accountId)
        {
            return await FindByCondition(a => a.Id.Equals(accountId))
                .Include(a=>a.Transactions)
                .FirstOrDefaultAsync();
        }
    }
}