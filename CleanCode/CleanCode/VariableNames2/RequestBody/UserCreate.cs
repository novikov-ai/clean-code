using System.Collections.Generic;
using Newtonsoft.Json;

namespace CleanCode.VariableNames2.RequestBody
{
    public class UserCreate: ICreate
    {
        [JsonProperty("firstName")] 
        public string FirstName { get; set; }
        
        [JsonProperty("lastName")] 
        public string LastName { get; set; }
        
        [JsonProperty("email")] 
        public string Email { get; set; }
    }
}