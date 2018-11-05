using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using spellbound_api.Models;

namespace spellbound_api.Controllers
{
  [Authorize]
  [Route("[controller]/[action]")]
  public class AccountController : ControllerBase
  {
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _roleManager = roleManager;
      _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<User>> SignIn([FromBody] LoginDto model)
    {
      var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
      if (result.Succeeded)
      {
        var user = _userManager.Users.FirstOrDefault(x => x.Email == model.Email);
        user.Token = GenerateJwtTocken(user);
        return Ok(user);
      }

      return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<User>> Register([FromBody] RegisterDto model)
    {
      var user = new User
      {
        Email = model.Email,
        UserName = model.Email
      };
      var role = new IdentityRole
      {
        Name = "user"
      };

      var result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        await _signInManager.SignInAsync(user, false);
        user.Token = GenerateJwtTocken(user);
        // await _userManager.AddToRoleAsync(user, "User");
        return Ok(user);
      }

      return StatusCode(500);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<User>> SignOut()
    {
      await _signInManager.SignOutAsync();
      return Ok();
    }

    private string GenerateJwtTocken(IdentityUser user)
    {
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, $"spellbound_token"),
        new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
        new Claim(JwtRegisteredClaimNames.Iss, _configuration["JwtIssuer"]),
        new Claim(JwtRegisteredClaimNames.Aud, _configuration["JwtAudience"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier,  user.Id)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

      var token = new JwtSecurityToken(null, null, claims, expires: expires, signingCredentials: creds);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}