using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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

    [NotMapped]
    public List<string> ClassTypes { get; set; }

    [JsonIgnore]
    [Required]
    [Column("ClassTypes")]
    private string ClassTypesSerialized
    {
      get => JsonConvert.SerializeObject(ClassTypes);
      set => ClassTypes = String.IsNullOrEmpty(value) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(value);
    }

    [Required]
    public string CastingTime { get; set; }

    [Required]
    public string Range { get; set; }

    [NotMapped]
    public List<string> Components { get; set; }

    [JsonIgnore]
    [Required]
    [Column("Components")]
    public string ComponentsSerialized
    {
      get => JsonConvert.SerializeObject(Components);
      set => Components = String.IsNullOrEmpty(value) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(value);
    }

    [Required]
    public string Duration { get; set; }

    public string Materials { get; set; }

    public string Description { get; set; }

    public string AtHigherLevels { get; set; }

    public string Reference { get; set; }

    public void LightlyLoad() {
      this.Description = null;
      this.AtHigherLevels = null;
      this.Reference = null;
    }
  }
}