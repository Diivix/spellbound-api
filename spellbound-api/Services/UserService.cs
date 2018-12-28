using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using spellbound_api.Models;

namespace spellbound_api.Services
{
    public class UserService
    {
        public static async Task<User> GetUserById(ApplicationDbContext context, string userId) 
        {
            return await context.Users
                .Include(x => x.Characters)
                .ThenInclude(x => x.CharacterSpells)
                .ThenInclude(x => x.Spell)
                .SingleOrDefaultAsync(x => x.Id == userId);
        }
    }
}