using System;
using Moq;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using spellbound_api.Services;
using spellbound_api.Controllers;
using spellbound_api.Models;
using System.Threading.Tasks;

namespace spellbound_api.tests
{
  public class SpellControllerTests
  {
    [Fact]
    public async Task SpellControllerIndexTest()
    {
      var mockDataService = new Mock<IDataService>();
      mockDataService.Setup(service => service.GetSpells()).ReturnsAsync(GetTestSpells());
      var controller = new SpellsController(mockDataService.Object);

      // var result = await controller.GetSpells();      
    }

    private List<Spell> GetTestSpells()
    {
      var spells = new List<Spell>();
      spells.Add(new Spell
      {
        Name = "Fire Ball",
        School = "Destruction",
        Level = 5,
        ClassTypes = new List<string>() { "wizard", "sorcerer" },
        CastingTime = "1 Action",
        Components = new List<string>() { "v" },
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
