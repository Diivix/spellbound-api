using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using spellbound_api.Models;

namespace spellbound_api.Services
{
    public class CharacterService
    {
        public static async Task<Character> GetById(ApplicationDbContext context, int characterId) 
        {
            return await context.Characters
                .Include(x => x.CharacterSpells)
                .ThenInclude(x => x.Spell)
                .SingleOrDefaultAsync(x => x.Id == characterId);
        }
    }
}