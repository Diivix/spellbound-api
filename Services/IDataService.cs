using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using spellbound_api.Models;

namespace spellbound_api.Services
{
  public interface IDataService
  {
    Task<IEnumerable<Spell>> GetSpells();
  }
}