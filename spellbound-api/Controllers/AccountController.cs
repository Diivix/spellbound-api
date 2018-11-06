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
using Newtonsoft.Json;
using spellbound_api.Models;

namespace spellbound_api.Controllers
{
  [Authorize(Policy = "UserAuth")]
  [Route("[controller]/[action]")]
  [ProducesResponseType(401)]
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
    [ProducesResponseType(500)]
    public async Task<ActionResult<User>> Register([FromBody] RegisterDto model)
    {
      var user = new User
      {
        Email = model.Email,
        UserName = model.Email
      };

      var result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        var roles = _roleManager.Roles.FirstOrDefault(x => x.NormalizedName == "USER");
        if (roles != null)
        {
          await _signInManager.SignInAsync(user, false);
          await _userManager.AddToRoleAsync(user, roles.Name);
          user.Token = GenerateJwtToken(user, roles.Name);
          return Ok(user);
        }
        return NotFound("User role not found.");
      }

      return StatusCode(500);
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
        var user = _userManager.Users.SingleOrDefault(x => x.Email == model.Email);
        if (user != null)
        {
          var roles = await _userManager.GetRolesAsync(user);
          user.Token = GenerateJwtToken(user, roles.ToList());
          return Ok(user);
        }
      }

      return Unauthorized();
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<User>> SignOut()
    {
      await _signInManager.SignOutAsync();
      return Ok();
    }

    private string GenerateJwtToken(User user, string role)
    {
      List<string> roles = new List<string> { role };
      return GenerateJwtToken(user, roles);
    }

    private string GenerateJwtToken(User user, List<string> roles)
    {
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, "spellbound_token"),
        new Claim(JwtRegisteredClaimNames.NameId, user.Id),
        new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
        new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      };

      roles.ForEach(x => claims.Add(new Claim("roles", x)));

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"]));

      var token = new JwtSecurityToken(null, null, claims, expires: expires, signingCredentials: creds);
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}