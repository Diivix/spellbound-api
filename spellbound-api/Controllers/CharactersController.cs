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
    [ProducesResponseType(typeof(CharacterViewModel), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Character>> Get([FromRoute] int? id)
    {
      if (id == null)
        return NotFound();

      var character = await CharacterService.GetById(_context, id.Value);
      if (character == null)
        return NotFound();

      var model = CharacterViewModel.Build(character);
      return Ok(model);
    }

    // GET api/characters/user
    [HttpGet("user/")]
    [ProducesResponseType(typeof(List<CharacterViewModel>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<Character>>> GetByUserId()
    {
      var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "nameid").Value;
      var user = await UserService.GetUserById(_context, userId);
      if (user == null)
        return NotFound("User not found.");
      
      // List<CharacterViewModel> model = new List<CharacterViewModel>();
      var model = user.Characters.Select(x => CharacterViewModel.Build(x));
      return Ok(model);
    }

    // POST api/characters
    [HttpPost]
    [ProducesResponseType(typeof(CharacterViewModel), 201)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Character>> Post([FromBody] Character character)
    {
      var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "nameid").Value;
      var user = await UserService.GetUserById(_context, userId);
      if (user == null)
        return NotFound("User not found.");

      character.CreatedDate = DateTime.Now;
      character.ModifiedDate = DateTime.Now;
      user.Characters.Add(character);
      await _context.SaveChangesAsync();

      var model = CharacterViewModel.Build(character);
      return Created(HttpContext.Request.Path, model);
    }

    // PUT api/characters
    [HttpPut]
    [ProducesResponseType(typeof(CharacterViewModel), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Character>> Put([FromBody] Character character)
    {
      var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "nameid").Value;
      var user = await UserService.GetUserById(_context, userId);
      if (user == null)
        return NotFound("User not found.");

      var existingCharacter = user.Characters.FirstOrDefault(x => x.Id == character.Id);
      if (existingCharacter == null)
        return NotFound("Character not found.");

      existingCharacter = character;
      existingCharacter.ModifiedDate = DateTime.Now;
      await _context.SaveChangesAsync();

      var model = CharacterViewModel.Build(existingCharacter);
      return Ok(model);
    }

    // PUT api/characters/addspell
    [HttpPut("addspell/")]
    [ProducesResponseType(typeof(CharacterViewModel), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)] // conflict
    public async Task<ActionResult<Character>> AddSpell([FromBody] CharacterAndSpellIds characterAndSpellIds)
    {
      var userId = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "nameid").Value;
      var user = await UserService.GetUserById(_context, userId);
      if (user == null)
        return NotFound("User not found.");

      var character = user.Characters.SingleOrDefault(x => x.Id == characterAndSpellIds.CharacterId);
      if (character == null)
        return NotFound("Character not found.");

      var spell = await SpellService.GetById(_context, characterAndSpellIds.SpellId);
      if (spell == null)
        return NotFound("Spell not found.");
      
      CharacterSpell characterSpell =  new CharacterSpell
      {
        Character = character,
        Spell = spell
      };

      var existingCharacterSpell = character.CharacterSpells.SingleOrDefault(x => x.SpellId == characterAndSpellIds.SpellId);
      if (existingCharacterSpell != null)
        return Conflict("Spell already added to character.");

      character.CharacterSpells.Add(characterSpell);
      character.ModifiedDate = DateTime.Now;
      await _context.SaveChangesAsync();

      var model = CharacterViewModel.Build(character);
      return Ok(model);
    }

    // PUT api/characters/removespell
    [HttpPut("removespell/")]
    [ProducesResponseType(typeof(CharacterViewModel), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Character>> RemoveSpell([FromBody] CharacterAndSpellIds characterAndSpellIds)
    {
      var userId = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "nameid").Value;
      var user = await UserService.GetUserById(_context, userId);
      if (user == null)
        return NotFound("User not found.");

      var character = user.Characters.SingleOrDefault(x => x.Id == characterAndSpellIds.CharacterId);
      if (character == null)
        return NotFound("Character not found.");

      var spell = await SpellService.GetById(_context, characterAndSpellIds.SpellId);
      if (spell == null)
        return NotFound("Spell not found.");

      var existingCharacterSpell = character.CharacterSpells.SingleOrDefault(x => x.SpellId == characterAndSpellIds.SpellId);
      if (existingCharacterSpell == null)
        return NotFound("Spell not found on character.");

      character.CharacterSpells.Remove(existingCharacterSpell);
      character.ModifiedDate = DateTime.Now;
      await _context.SaveChangesAsync();

      var model = CharacterViewModel.Build(character);
      return Ok(model);
    }
    public class CharacterAndSpellIds
    {
      public int CharacterId { get; set; }
      public int SpellId { get; set; }
    }
  }
}