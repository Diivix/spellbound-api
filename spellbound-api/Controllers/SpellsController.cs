using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using spellbound_api.Models;
using spellbound_api.Services;

namespace spellbound_api.Controllers
{
  [Route("api/[controller]")]
  public class SpellsController : ControllerBase
  {
    private readonly IDataService _dataservice;

    public SpellsController(IDataService dataService) {
      _dataservice = dataService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Spell>), 200)]
    public async Task<ActionResult<IEnumerable<Spell>>> GetSpells()
    {
      var docs = await _dataservice.GetSpells();
      return Ok(docs);
    }
  }
}