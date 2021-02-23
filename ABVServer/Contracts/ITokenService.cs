using Entities.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITokenService
    {
        Task<string> GetToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
