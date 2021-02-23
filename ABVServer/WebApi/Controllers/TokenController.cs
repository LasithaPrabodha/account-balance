using Contracts;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILoggerManager _logger;

        public TokenController(UserManager<User> userManager, ITokenService tokenService, ILoggerManager logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto tokenDto)
        {
            try
            {

                if (tokenDto is null)
                    return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });


                var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
                var username = principal.Identity.Name;

                var user = await _userManager.FindByEmailAsync(username);
                if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                    return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });

                var token = await _tokenService.GetToken(user);
             
                user.RefreshToken = _tokenService.GenerateRefreshToken();

                await _userManager.UpdateAsync(user);

                return Ok(new AuthResponseDto { AccessToken = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new Error { Message = e.Message });
            }
        }
    }
}
