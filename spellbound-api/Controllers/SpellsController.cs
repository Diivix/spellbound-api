using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spellbound_api.Models;
using spellbound_api.Services;

namespace spellbound_api.Controllers
{
  [Route("api/[controller]")]
  public class SpellsController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public SpellsController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET api/spells/{id}
    // [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IEnumerable<Spell>), 200)]
    public async Task<ActionResult<IEnumerable<Spell>>> Get([FromRoute] int? id)
    {
      if (id == null)
        return NotFound();

      var spell = await _context.Spells.FirstOrDefaultAsync(x => x.Id == id);
      if (spell == null)
        return NotFound(); 

      return Ok(spell);
    }

    // GET api/spells/all?partial=false
    // [Authorize]
    [HttpGet("all/")]
    [ProducesResponseType(typeof(IEnumerable<Spell>), 200)]
    public async Task<ActionResult<IEnumerable<Spell>>> GetAll([FromQuery] string partial="false")
    {
      var spells = await _context.Spells.ToListAsync();
      if (!partial.Equals("true"))
      {
        return Ok(spells);
      }

      spells.ForEach(x => x.LightlyLoad());
      return Ok(spells);
    }

    // POST api/spells
    // [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<Spell>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] IEnumerable<Spell> spells)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      await _context.Spells.AddRangeAsync(spells);
      _context.SaveChanges();
      return Created("Post", spells);
    }
  }
}