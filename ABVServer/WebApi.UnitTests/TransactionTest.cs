using AutoMapper;
using Contracts;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApi.Controllers;
using Xunit;

namespace WebApi.UnitTests
{
    public class TransactionTest
    {
        [Fact]
        [Trait("Transactions", "Transactions_Get")]
        public void Transactions_Get()
        {
            // ARRANGE
            Mock<ITransactionRepository> repositoryMock = new Mock<ITransactionRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILoggerManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);


            var mockTransactions = new List<Transaction> {
                new Transaction { Id = 1, AccountId = 1, AddedDateTime= DateTime.Now, Amount = 10, TransactionDate= DateTime.Now},
                new Transaction { Id = 2, AccountId = 2, AddedDateTime= DateTime.Now, Amount = 20, TransactionDate= DateTime.Now},
                new Transaction { Id = 3, AccountId = 3, AddedDateTime= DateTime.Now, Amount = 30, TransactionDate= DateTime.Now},
                new Transaction { Id = 4, AccountId = 4, AddedDateTime= DateTime.Now, Amount = 40, TransactionDate= DateTime.Now},
                new Transaction { Id = 5, AccountId = 5, AddedDateTime= DateTime.Now, Amount = 50, TransactionDate= DateTime.Now},
            }.AsQueryable();

            unitOfWork.Setup(x => x.Transaction).Returns(repositoryMock.Object);
            unitOfWork.Setup(x => x.Transaction.FindAll()).Returns(mockTransactions);

            var controller = new TransactionController(mockLogger.Object, unitOfWork.Object, mapper);

            // ACT
            IActionResult result = controller.Get();
            ObjectResult objectResponse = result as ObjectResult;

            // ASSERT
            Assert.Equal(200, objectResponse.StatusCode);

            List<TransactionDto> accountResponse = Assert.IsAssignableFrom<List<TransactionDto>>(objectResponse.Value);

            Assert.True(accountResponse.Count > 0);

        }

        [Theory]
        [Trait("Transactions", "Transactions_Get_For_Range")]
        [InlineData("Wed, 01 May 2019 08:44:05 GMT", "Mon, 01 Feb 2021 08:44:05 GMT")]
        public void Transactions_Get_For_Range(string startDate, string endDate)
        {

            // ARRANGE
            Mock<ITransactionRepository> transRepoMock = new Mock<ITransactionRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILoggerManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            unitOfWork.Setup(x => x.Transaction)
                .Returns(transRepoMock.Object);

            unitOfWork.Setup(x => x.Transaction.FindByCondition(It.IsAny<Expression<Func<Transaction, bool>>>()))
                .Returns(It.IsAny<IQueryable<Transaction>>());

            var controller = new TransactionController(mockLogger.Object, unitOfWork.Object, mapper);

            // ACT
            var result = controller.GetTransactionForRange(startDate, endDate);
            ObjectResult objectResponse = result as ObjectResult;

            // ASERT
            Assert.Equal(200, objectResponse.StatusCode);
        }


        [Theory]
        [Trait("Transactions", "Transactions_Get_For_Range_Bad_Request")]
        [InlineData("Mon, 01 Feb 2021 08:44:05 GMT", "Wed, 01 May 2019 08:44:05 GMT")]
        public void Transactions_Get_For_Range_Bad_Request(string startDate, string endDate)
        {

            // ARRANGE
            var unitOfWork = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILoggerManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var controller = new TransactionController(mockLogger.Object, unitOfWork.Object, mapper);

            // ACT
            var result = controller.GetTransactionForRange(startDate, endDate);
            ObjectResult objectResponse = result as ObjectResult;

            // ASERT
            Assert.Equal(400, objectResponse.StatusCode);
        }

    }
}
