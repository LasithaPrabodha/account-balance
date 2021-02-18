using Contracts;
using Entities;
using Entities.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Account GetAccountById(int accountId)
        {
            return FindByCondition(a => a.Id.Equals(accountId))
                .FirstOrDefault();
        }

        public Account GetAccountWithDetails(int accountId)
        {
            return FindByCondition(a => a.Id.Equals(accountId))
                .Include(a=>a.Transactions)
                .FirstOrDefault();
        }
    }
}