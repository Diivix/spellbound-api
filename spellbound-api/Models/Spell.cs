using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace spellbound_api.Models
{
  public class Spell : Entity
  {
    [Required]
    public string Name { get; set; }

    [Required]
    public string School { get; set; }

    [Required]
    public int Level { get; set; }

    [Required]
    public List<ClassType> ClassTypes { get; set; }

    [Required]
    public string CastingTime { get; set; }

    [Required]
    public string Range { get; set; }

    [Required]
    public List<Component> Components { get; set; }

    [Required]
    public string Duration { get; set; }

    [Required]
    public string Materials { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string AtHigherLevels { get; set; }

    [Required]
    public string Reference { get; set; }
  }
}