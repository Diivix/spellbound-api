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
  [Authorize]
  [Route("api/[controller]")]
  public class SpellsController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public SpellsController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET api/spells/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IEnumerable<Spell>), 200)]
    [ProducesResponseType(404)]
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
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Spell>), 200)]
    public async Task<ActionResult<IEnumerable<Spell>>> Get([FromQuery] string partial = "false")
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
    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<Spell>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] List<Spell> spells)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      spells.ForEach(x => { x.CreatedDate = DateTime.Now; x.ModifiedDate = DateTime.Now; });
      await _context.Spells.AddRangeAsync(spells);
      _context.SaveChanges();
      return Created("Post", spells);
    }

    // GET api/spells/all
    [HttpDelete]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<Spell>>> Delete()
    {
      _context.Spells.RemoveRange(_context.Spells);
      await _context.SaveChangesAsync();
      return Ok();
    }
  }
}