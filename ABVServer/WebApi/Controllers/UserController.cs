using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using System.Linq;
using Contracts;
using System;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly ITokenService _tokenService;

        public UserController(UserManager<User> userManager, IMapper mapper, ILoggerManager logger, ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            try
            {

                if (userForRegistration == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                User user = _mapper.Map<User>(userForRegistration);

                IdentityResult result = await _userManager.CreateAsync(user, userForRegistration.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);

                    return BadRequest(new RegistrationResponseDto { Errors = errors });
                }

                if (userForRegistration.Role == Roles.Administrator)
                {
                    await _userManager.AddToRoleAsync(user, "Administrator");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Viewer");
                }


                return Ok(new RegistrationResponseDto { IsSuccessfulRegistration = true });
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return StatusCode(500, new Error { Message = e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userForAuthentication.Email);

                if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

                var signingCredentials = _tokenService.GetSigningCredentials();
                var claims = await _tokenService.GetClaims(user);
                var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                user.RefreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                await _userManager.UpdateAsync(user);

                return Ok(new AuthResponseDto { IsAuthSuccessful = true, AccessToken = token, RefreshToken = user.RefreshToken });
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return StatusCode(500, new Error { Message = e.Message });
            }
        }
    }
}
