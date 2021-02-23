using AutoMapper;
using Contracts;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using WebApi.Controllers;
using Xunit;

namespace WebApi.UnitTests
{
    public class UserRegisterTest
    {
        [Theory]
        [Trait("Registration", "Registration_Successful")]
        [MemberData(nameof(TestDataGenerator.GetRegisterData), MemberType = typeof(TestDataGenerator))]
        public async void Registration_Successful(string fname, string lname, string password, string confirmPassword, string email, Roles role)
        {
            // ARRANGE
            var userForReg = new UserForRegistrationDto
            {
                FirstName = fname,
                LastName = lname,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                Role = role
            };

            var mockUserManager = new Mock<FakeUserManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var mockLogger = new Mock<ILoggerManager>();

            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);
            mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
               .ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);
            var controller = new UserController(mockUserManager.Object, null, mapper, mockLogger.Object, null);

            // ACT
            IActionResult result = await controller.RegisterUser(userForReg);
            ObjectResult objectResponse = result as ObjectResult;

            // ASSERT
            Assert.Equal(200, objectResponse.StatusCode);
            RegistrationResponseDto regResponse = Assert.IsType<RegistrationResponseDto>(objectResponse.Value);

            Assert.True(regResponse.IsSuccessfulRegistration);
        }
    }
}
