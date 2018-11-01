using System;
using Moq;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using spellbound_api.Services;
using spellbound_api.Controllers;
using spellbound_api.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace spellbound_api.tests
{
  public class SpellControllerTests
  {
    [Fact]
    public async Task SpellControllerIndexTest()
    {
      // var context = new Mock<SqliteContext>();
      // context.Setup();
      // var controller = new SpellsController(mockDataService.Object);

      // var result = await controller.GetSpells();      
    }

    private List<Spell> GetTestSpells()
    {
      var classTypes = new List<ClassType>(){};
      var type1 = new ClassType 
      {
        Type = "wizard"
      };
      var type2 = new ClassType 
      {
        Type = "bard"
      };
      classTypes.Add(type1);
      classTypes.Add(type2);

      var components = new List<Component>(){};
      var component1 = new Component 
      {
        Type = "v"
      };
      var component2 = new Component 
      {
        Type = "s"
      };
      components.Add(component1);
      components.Add(component2);

      var spells = new List<Spell>();
      spells.Add(new Spell
      {
        Name = "Fire Ball",
        School = "Destruction",
        Level = 5,
        ClassTypes = classTypes,
        CastingTime = "1 Action",
        Components = components,
        Range = "120 feet",
        Duration = "Action",
        Materials = "",
        Description = "Shoots a fire ball at the target.",
        AtHigherLevels = "",
        Reference = "Page 10"
      });
      return spells;
    }
  }
}
