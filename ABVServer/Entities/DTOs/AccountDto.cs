using System;

namespace Entities.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public DateTime DateCreated { get; set; }
        public double Balance { get; set; }
    }
}
