using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace WebApi.UnitTests
{
    public class FakeSignInManager : SignInManager<User>
    {
        public FakeSignInManager(IHttpContextAccessor contextAccessor)
              : base(new FakeUserManager(),
                   contextAccessor,
                   new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                   new Mock<IOptions<IdentityOptions>>().Object,
                   new Mock<ILogger<SignInManager<User>>>().Object,
                   new Mock<IAuthenticationSchemeProvider>().Object, 
                   null)
        {
        }
    }
}
