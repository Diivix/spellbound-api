using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace spellbound_api.Models
{
  public class Character : Entity
  {
    [Required]
    public string Name { get; set; }
    public string Level { get; set; }
    public string ClassType { get; set; }
    public string Description { get; set; }
    [JsonProperty("spells")]
    public List<CharacterSpell> CharacterSpells { get; set; }
  }
}