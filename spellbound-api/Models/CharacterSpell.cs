using Newtonsoft.Json;

namespace spellbound_api.Models
{
  public class CharacterSpell
  {
    [JsonIgnore]
    public int CharacterId { get; set; }
    [JsonIgnore]
    public Character Character { get; set; }
    [JsonIgnore]
    public int SpellId { get; set; }
    public Spell Spell { get; set; }
  }
}