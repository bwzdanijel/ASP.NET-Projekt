using Microsoft.AspNetCore.Authorization;
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
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : Controller
	{
		private readonly AppContext _context;

		public AdminController(AppContext context)
		{
			_context = context;
		}

		[HttpPost("adminLogin")]
		public IActionResult Login(LoginInfo login)
		{
			Admin adminInDb = _context.Admins.FirstOrDefault(admin => admin.Username == login.Username);
			if (adminInDb == null && HashGenerator.VerifyHash(adminInDb.Password, login.Password, adminInDb.Salt))
			{

				//return RedirectToPage("/pages/login.html");
				return Ok(CreateToken(adminInDb.Id, adminInDb.Username));
			}
			return Unauthorized();
		}

		/*
        [HttpPost("adminLogin")]
        public IActionResult Login(LoginInfo login)
        {
            Admin adminInDb = _context.Admins.FirstOrDefault(admin => admin.Username == login.Username);
            if (adminInDb != null && HashGenerator.VerifyHash(adminInDb.Password, login.Password, adminInDb.Salt))
            {
                return RedirectToPage("/admin");
                //return Ok(CreateToken(adminInDb.Id, adminInDb.Username));
            }
            return Unauthorized();
        }*/


        [Authorize]
		[HttpGet]
		public IActionResult Index()
		{
			var users = _context.Users.ToList();
			return View(users);
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
