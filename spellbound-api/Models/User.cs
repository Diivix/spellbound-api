using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace spellbound_api.Models
{
  // Only returned properties that have been opted-ed into the client
  [JsonObject(MemberSerialization.OptIn)]
  public class User : IdentityUser
  {
    [JsonProperty]
    public override string UserName { get; set; }
    
    [PersonalData]
    [JsonProperty]
    public List<Character> Characters { get; set; }
  }
}