﻿using System;

namespace Entities.DTOs
{
    public class TransactionWithDetailsDto
    {
        public int Id { get; set; }
        public DateTime AddedDateTime { get; set; }
        public double Amount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public AccountDto Account { get; set; }
    }

}
