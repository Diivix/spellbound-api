using System;

namespace spellbound_api.Models
{
  public abstract class Entity
  {
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
  }
}