using System;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class AccountWithDetailsDto
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public DateTime DateCreated { get; set; }
        public double Balance { get; set; }
        public IEnumerable<TransactionDto> Transactions { get; set; }
    }
}
