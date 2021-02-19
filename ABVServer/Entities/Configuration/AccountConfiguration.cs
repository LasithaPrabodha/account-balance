using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasData(
                  new Account { Id = 1, AccountName = "Rnd", DateCreated = DateTime.Now, Balance = 0 },
                  new Account { Id = 2, AccountName = "Canteen", DateCreated = DateTime.Now, Balance = 0 },
                  new Account { Id = 3, AccountName = "CEO\'s Car", DateCreated = DateTime.Now, Balance = 0 },
                  new Account { Id = 4, AccountName = "Marketing", DateCreated = DateTime.Now, Balance = 0 },
                  new Account { Id = 5, AccountName = "Parking Fines", DateCreated = DateTime.Now, Balance = 0 }
                );
        }
    }
}
