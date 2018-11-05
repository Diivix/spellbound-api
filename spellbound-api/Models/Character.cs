using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace spellbound_api.Models
{
  public class Character : Entity
  {
    [Required]
    public string Name { get; set; }
    public string Level { get; set; }
    public string ClassType { get; set; }
    public string Description { get; set; }
    public List<CharacterSpell> CharacterSpells { get; set; }
  }
}