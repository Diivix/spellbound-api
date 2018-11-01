using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spellbound_api.Models;
using spellbound_api.Services;

namespace spellbound_api.Controllers
{
  [Route("api/[controller]")]
  public class SpellsController : ControllerBase
  {
      private readonly SqliteContext _context;

    public SpellsController(SqliteContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Spell>), 200)]
    public async Task<ActionResult<IEnumerable<Spell>>> GetSpells()
    {
      var spells = await _context.Spells.ToListAsync();
      return Ok(spells);
    }
  }
}