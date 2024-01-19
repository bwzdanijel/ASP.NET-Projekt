using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjektArbeitDanijelMademidda.Data;
using ProjektArbeitDanijelMademidda.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppContext = ProjektArbeitDanijelMademidda.Models.AppContext;

namespace ProjektArbeitDanijelMademidda.Controllers
{

    public class LoginInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserToken
    {
        public string Username { get; set; }
        public string JWT { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly AppContext _context;
        public UsersController(AppContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(LoginInfo login)
        {
            User userInDb = _context.Users.FirstOrDefault(user => user.Username == login.Username);
            if (userInDb == null)
            {
                string salt;
                string pwHash = HashGenerator.GenerateHash(login.Password, out salt);
                User newUser = new User() { Username = login.Username, Password = pwHash, Salt = salt };

                newUser.UserIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                _context.Users.Add(newUser);
                _context.SaveChanges();
                return Ok(CreateToken(newUser.Id, newUser.Username));

				return RedirectToAction("Index", "Home");
			}
            return BadRequest();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginInfo login)
        {
            User userInDb = _context.Users.FirstOrDefault(user => user.Username == login.Username);
            if (userInDb == null && HashGenerator.VerifyHash(userInDb.Password, login.Password, userInDb.Salt))
            {
                return Ok(CreateToken(userInDb.Id, userInDb.Username));
            }
            return Unauthorized();
        }

        private UserToken CreateToken(long userId, string username)
        {
            var expires = DateTime.UtcNow.AddDays(5);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, $"{userId}"),
                    new Claim(JwtRegisteredClaimNames.UniqueName, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = expires,
                Issuer = JwtConfiguration.ValidIssuer,
                Audience = JwtConfiguration.ValidAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(JwtConfiguration.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return new UserToken { Username = username, ExpiresAt = expires, JWT = jwtToken };  

            
        }
    }
}
