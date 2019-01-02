using System;
using System.Collections.Generic;
using System.Linq;

namespace spellbound_api.Models
{
  /// <summary>
  /// The class is the same as the Character class, but without the nesting caused by the CharacterSpell class.
  /// </summary>
  public class CharacterViewModel
  {
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string Name { get; set; }
    public string Level { get; set; }
    public string ClassType { get; set; }
    public string Description { get; set; }
    public List<Spell> Spells { get; set; }

    public static CharacterViewModel Build(Character character) {
        var model = new CharacterViewModel {
            Id = character.Id,
            CreatedDate = character.CreatedDate,
            ModifiedDate = character.ModifiedDate,
            Name = character.Name,
            ClassType = character.ClassType,
            Level = character.Level,
            Description = character.Description,
            Spells = character.CharacterSpells?.Select(x => x.Spell).ToList()
        };
        
        model.Spells?.ForEach(x => x.LightlyLoad());
        return model;
    }
  }
}