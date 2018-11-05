namespace spellbound_api.Models
{
    public class CharacterSpell
    {
        public int CharacterId {get;set;}
        public Character Character {get;set;}
        public int SpellId {get;set;}
        public Spell Spell {get; set;}
    }
}