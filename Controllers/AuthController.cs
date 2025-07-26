using DoItBetterCoreAPI.Dtos;
using DoItBetterCoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoItBetterCoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOptions<AppSettings> _appSettings;

        public AuthController(UserManager<AppUser> userManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] UserRegistrationDto userRegistrationDto)
        {
            AppUser user = new AppUser()
            {
                UserName = userRegistrationDto.Email.Split("@")[0],
                Email = userRegistrationDto.Email,
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
            };
            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] UserLoginDto userRegistrationDto)
        {
            var user = await _userManager.FindByEmailAsync(userRegistrationDto.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Username or Password is incorrect." });
            }

            if (!await _userManager.CheckPasswordAsync(user, userRegistrationDto.Password))
            {
                return BadRequest(new { message = "Username or Password is incorrect." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.JWTSecret));

            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim("userID", user.Id.ToString()),
                new Claim(ClaimTypes.Role, roles.First()),
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return Ok(new { token });
        }
    }
}
