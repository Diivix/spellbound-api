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
  public class CharacterController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public CharacterController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET api/character/{id}
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
  }
}