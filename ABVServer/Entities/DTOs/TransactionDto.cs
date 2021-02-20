using System;

namespace Entities.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public DateTime AddedDateTime { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }
        public int AccountId { get; set; }
    }

}
