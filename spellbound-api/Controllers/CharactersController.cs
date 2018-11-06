using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spellbound_api.Models;

namespace spellbound_api.Controllers
{
  [Authorize(Policy = "UserAuth")]
  [Route("api/[controller]")]
  [ProducesResponseType(401)]
  public class CharactersController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public CharactersController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET api/characters/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Character), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Character>> Get([FromRoute] int? id)
    {
      if (id == null)
        return NotFound();

      var character = await _context.Characters.FirstOrDefaultAsync(x => x.Id == id);
      if (character == null)
        return NotFound();

      return Ok(character);
    }

    // GET api/characters/user/{id}
    [HttpGet("user/")]
    [ProducesResponseType(typeof(List<Character>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<Character>>> GetByUserId()
    {
      var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "nameid").Value;
      var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
      if (user == null)
        return NotFound("User not found.");

      return Ok(user.Characters);
    }

    // POST api/characters
    [HttpPost]
    [ProducesResponseType(typeof(List<Character>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Character>> Post([FromBody] Character character)
    {
      var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "nameid").Value;
      var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
      if (user == null)
        return NotFound("User not found.");

      character.CreatedDate = DateTime.Now;
      character.ModifiedDate = DateTime.Now;
      user.Characters.Add(character);
      await _context.SaveChangesAsync();
      return Created(HttpContext.Request.Path, character);
    }

    // PUT api/characters
    [HttpPut]
    [ProducesResponseType(typeof(List<Character>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Character>> Put([FromBody] Character character)
    {
      var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "nameid").Value;
      var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
      if (user == null)
        return NotFound("User not found.");

      var existingCharacter = user.Characters.FirstOrDefault(x => x.Id == character.Id);
      if (existingCharacter == null)
        return NotFound("Character not found.");

      existingCharacter = character;
      existingCharacter.ModifiedDate = DateTime.Now;
      await _context.SaveChangesAsync();
      return Ok(character);
    }
  }
}