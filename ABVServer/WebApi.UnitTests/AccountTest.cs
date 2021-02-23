using AutoMapper;
using Contracts;
using CsvHelper;
using CsvHelper.Configuration;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WebApi.Controllers;
using Xunit;

namespace WebApi.UnitTests
{
    public class AccountTest
    {

        [Fact]
        [Trait("Accounts", "Accounts_Get")]
        public void Accounts_Get()
        {
            // ARRANGE
            Mock<IAccountRepository> repositoryMock = new Mock<IAccountRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILoggerManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);


            var accounts = new List<Account> {
                new Account { Id = 1, AccountName = "Rnd", DateCreated = DateTime.Now, Balance = 0 },
                new Account { Id = 2, AccountName = "Canteen", DateCreated = DateTime.Now, Balance = 0 },
                new Account { Id = 3, AccountName = "CEO\'s Car", DateCreated = DateTime.Now, Balance = 0 },
                new Account { Id = 4, AccountName = "Marketing", DateCreated = DateTime.Now, Balance = 0 },
                new Account { Id = 5, AccountName = "Parking Fines", DateCreated = DateTime.Now, Balance = 0 }
            }.AsQueryable();

            unitOfWork.Setup(x => x.Account).Returns(repositoryMock.Object);
            unitOfWork.Setup(x => x.Account.FindAll()).Returns(accounts);

            var controller = new AccountController(mockLogger.Object, unitOfWork.Object, mapper);

            // ACT
            IActionResult result = controller.Get();
            ObjectResult objectResponse = result as ObjectResult;

            // ASSERT
            Assert.Equal(200, objectResponse.StatusCode);

            List<AccountDto> accountResponse = Assert.IsAssignableFrom<List<AccountDto>>(objectResponse.Value);

            Assert.True(accountResponse.Count == 5);

        }

        [Theory]
        [Trait("Accounts", "Accounts_Update")]
        [InlineData("Mon, 01 Feb 2021 08:44:05 GMT")]
        public async void Accounts_Update(string transactionDate)
        {
            // ARRANGE
            Mock<IAccountRepository> accountRepoMock = new Mock<IAccountRepository>();
            Mock<ITransactionRepository> transRepoMock = new Mock<ITransactionRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILoggerManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);


            unitOfWork.Setup(x => x.Account)
                .Returns(accountRepoMock.Object);
            unitOfWork.Setup(x => x.Transaction)
                .Returns(transRepoMock.Object);

            var accounts = new List<Account>() {
                new Account { Id = 1, AccountName = "Rnd", DateCreated = DateTime.Now, Balance = 0 },
                new Account { Id = 2, AccountName = "Canteen", DateCreated = DateTime.Now, Balance = 0 },
                new Account { Id = 3, AccountName = "CEO\'s Car", DateCreated = DateTime.Now, Balance = 0 },
                new Account { Id = 4, AccountName = "Marketing", DateCreated = DateTime.Now, Balance = 0 },
                new Account { Id = 5, AccountName = "Parking Fines", DateCreated = DateTime.Now, Balance = 0 }
            };

            var transactions = new List<Transaction>()
            {

            };

            var builtMockAccounts = accounts.AsQueryable().BuildMock();
            var builtMockTrans = transactions.AsQueryable().BuildMock();


            unitOfWork.Setup(x => x.Account.FindByCondition(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(builtMockAccounts.Object);
            unitOfWork.Setup(x => x.Transaction.FindByCondition(It.IsAny<Expression<Func<Transaction, bool>>>()))
                .Returns(builtMockTrans.Object);

            unitOfWork.Setup(x => x.Account.Update(It.IsAny<Account>()));
            unitOfWork.Setup(x => x.Transaction.Create(It.IsAny<Transaction>()));

            var controller = new AccountController(mockLogger.Object, unitOfWork.Object, mapper);

            var data = new[]
           {
                new { Name = "RnD", Amount = 20 },
                new { Name = "Canteen", Amount = 40 },
                new { Name = "CEO's Car", Amount = 1003 },
                new { Name = "Marketing", Amount = 302 },
                new { Name = "Parking Fines", Amount = -100 }
            };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                HasHeaderRecord = false,
            };

            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, config))
            {

                csvWriter.WriteRecords(data);

                writer.Flush();
                IFormFile file = new FormFile(mem, 0, mem.Length, "Data", "dummy.csv");


                // ACT
                var result = await controller.UpdateAccounts(new UploadBalance { File = file, TransactionDate = transactionDate });
                ObjectResult objectResponse = result as ObjectResult;

                // ASERT
                Assert.Equal(200, objectResponse.StatusCode);

            }





        }

        [Theory]
        [Trait("Accounts", "Accounts_Update_Wrong_File_Format")]
        [InlineData("Mon, 01 Feb 2021 08:44:05 GMT")]
        public async void Accounts_Update_Wrong_File_Format(string transactionDate)
        {
            // ARRANGE
            var controller = new AccountController(null, null, null);
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

            // ACT
            var result = await controller.UpdateAccounts(new UploadBalance { File = file, TransactionDate = transactionDate });
            ObjectResult objectResponse = result as ObjectResult;

            // ASERT
            Assert.Equal(400, objectResponse.StatusCode);

        }


        [Theory]
        [Trait("Accounts", "Accounts_Update_Invalid_Date")]
        [InlineData("2021-13")]
        public async void Accounts_Update_Invalid_Date(string transactionDate)
        {
            // ARRANGE
            var controller = new AccountController(null, null, null);
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

            // ACT
            var result = await controller.UpdateAccounts(new UploadBalance { File = file, TransactionDate = transactionDate });
            ObjectResult objectResponse = result as ObjectResult;

            // ASERT
            Assert.Equal(400, objectResponse.StatusCode);

        }
    }
}
