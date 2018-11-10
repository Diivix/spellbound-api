using Newtonsoft.Json;

namespace spellbound_api.Models
{
    public class UserWithToken
    {
        [JsonProperty]
        public string Token {get;set;}
        [JsonProperty]
        public User User {get;set;}
    }
}