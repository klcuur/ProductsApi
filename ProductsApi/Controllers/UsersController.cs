using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductsApi.Dto;
using ProductsApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductsApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IConfiguration _config;

		public UsersController(UserManager<AppUser> userManager,SignInManager<AppUser>signInManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_config = configuration;
		}
		[HttpPost("register")]
		public async Task<IActionResult> CreateUser(UserDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var user = new AppUser
			{
				UserName = model.UserName,
				Email = model.Email,
				FullName = model.FullName,
				DateAdded = DateTime.Now
			};
			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				return StatusCode(201);
			}
			return BadRequest(result.Errors);

		}
		[HttpPost("login")]
		public async Task<IActionResult>Login(LoginDto model)
		{
			var user=await _userManager.FindByEmailAsync(model.Email);

			if (user == null)
			{
				return BadRequest(new { message = "email hatali" });
			}

			var result=await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);

			if (result.Succeeded)
			{
				return Ok(new {token= GenerateJwt(user)});
			}
			return Unauthorized();
		}
		private object GenerateJwt(AppUser user)
		{
			var tokendHandler = new JwtSecurityTokenHandler();
			var key=Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Secret").Value ?? "");
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.UserName ?? ""),
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),
				Issuer="info@ugurkilic.com"
			};
			var token = tokendHandler.CreateToken(tokenDescriptor);
			return tokendHandler.WriteToken(token);
		}
	}
}
