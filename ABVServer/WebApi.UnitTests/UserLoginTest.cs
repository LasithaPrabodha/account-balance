using AutoMapper;
using Contracts;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using WebApi.Controllers;
using Xunit;

namespace WebApi.UnitTests
{
    public class UserLoginTest
    {

        [Theory]
        [Trait("Login", "Login_Successful")]
        [InlineData("testuser@test.com", "Tester@#12345")]
        public async void Login_Successful(string userName, string password)
        {

            // ARRANGE
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Test",
                LastName = "Test",
                UserName = userName,
                Email = userName,
            };

            var mockContextAssosor = new Mock<IHttpContextAccessor>();
            var mockSignInManager = new Mock<FakeSignInManager>(mockContextAssosor.Object);
            var mockUserManager = new Mock<FakeUserManager>();

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var mockLogger = new Mock<ILoggerManager>();
            var mockTokenService= new Mock<ITokenService>();

            mockSignInManager.Setup(m => m.PasswordSignInAsync(userName, password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            mockUserManager.Setup(x => x.FindByEmailAsync(userName))
                .ReturnsAsync(new User { UserName = userName }); 
            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);
            mockTokenService.Setup(x => x.GetToken(It.IsAny<User>()))
                .ReturnsAsync("token");

            var controller = new UserController(mockUserManager.Object, mockSignInManager.Object, mapper, mockLogger.Object, mockTokenService.Object);
            
            // ACT
            IActionResult result = await controller.Login(new UserForAuthenticationDto { Email = userName, Password = password });
            ObjectResult objectResponse = result as ObjectResult;

            // ASSERT
            Assert.Equal(200, objectResponse.StatusCode);

            AuthResponseDto authResponse = Assert.IsType<AuthResponseDto>(objectResponse.Value);

            Assert.NotNull(authResponse.AccessToken);
            Assert.True(authResponse.IsAuthSuccessful);


        }

        [Theory]
        [Trait("Login", "Login_WrongPassword")]
        [InlineData("testuser@test.com", "Tester@#12345")]
        public async void Login_WrongPassword(string userName, string password)
        {

            // ARRANGE
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Test",
                LastName = "Test",
                UserName = userName,
                Email = userName,
            };

            var mockSignInManager = new Mock<FakeSignInManager>(new Mock<IHttpContextAccessor>().Object);
            var mockUserManager = new Mock<FakeUserManager>();
            var mockLogger = new Mock<ILoggerManager>();
            var mockTokenService = new Mock<ITokenService>();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            mockSignInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var controller = new UserController(mockUserManager.Object, mockSignInManager.Object, mapper, mockLogger.Object, mockTokenService.Object);

            // ACT
            IActionResult result = await controller.Login(new UserForAuthenticationDto { Email = userName, Password = password });
            ObjectResult objectResponse = result as ObjectResult;

            // ASSERT
            Assert.Equal(401, objectResponse.StatusCode);

            AuthResponseDto authResponse = Assert.IsType<AuthResponseDto>(objectResponse.Value);

            Assert.Null(authResponse.AccessToken);
            Assert.False(authResponse.IsAuthSuccessful);


        }
    }
}
