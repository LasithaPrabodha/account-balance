using Contracts;
using Entities;
using Entities.Models;
using System.Collections.Generic;

namespace Repository
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

     
    }
}