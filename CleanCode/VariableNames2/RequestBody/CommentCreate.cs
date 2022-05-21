using System.Collections.Generic;
using Newtonsoft.Json;

namespace CleanCode.VariableNames2.RequestBody
{
    public class CommentCreate
    {
        [JsonProperty("message")] 
        public string Message { get; set; }
        
        [JsonProperty("post")] 
        public string PostId { get; set; }

        [JsonProperty("owner")] 
        public string OwnerId { get; set; }
    }
}