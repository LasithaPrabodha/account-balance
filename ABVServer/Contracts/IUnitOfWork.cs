using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUnitOfWork
    {
        IAccountRepository Account { get; }
        ITransactionRepository Transaction { get; }
        Task Save();
    }
}