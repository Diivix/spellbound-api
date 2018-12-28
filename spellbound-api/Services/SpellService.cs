using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using spellbound_api.Models;

namespace spellbound_api.Services
{
    public class SpellService
    {
        public static async Task<Spell> GetById(ApplicationDbContext context, int spellId) 
        {
            return await context.Spells.SingleOrDefaultAsync(x => x.Id == spellId);
        }

        public static async Task<List<Spell>> GetAll(ApplicationDbContext context) 
        {
            return await context.Spells.ToListAsync();
        }
    }
}