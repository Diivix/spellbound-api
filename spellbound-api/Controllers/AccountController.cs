using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using spellbound_api.Models;

namespace spellbound_api.Controllers
{
  [Route("[controller]/[action]")]
  public class AccountController : ControllerBase
  {
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _configuration = configuration;
    }

    [HttpPost]
    public async Task<Object> Login([FromBody] LoginDto model)
    {
      var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
      if (result.Succeeded)
      {
        var user = _userManager.Users.FirstOrDefault(x => x.Email == model.Email);
        return GenerateJwtTocken(user);
      }

      throw new UnauthorizedAccessException();
    }

    [HttpPost]
    public async Task<Object> Register([FromBody] RegisterDto model) {
        var user = new IdentityUser {
            Email = model.Email,
            UserName = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if(result.Succeeded) 
        {
            await _signInManager.SignInAsync(user, false);
            return GenerateJwtTocken(user);
        }

        throw new SystemException();
    }

    private object GenerateJwtTocken(IdentityUser user)
    {
      var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
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