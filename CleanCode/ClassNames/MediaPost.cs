using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CleanCode.ClassNames
{
    // 3.1 (3)
    // PostCreate - MediaPost
    public class MediaPost
    {
        [JsonProperty("text")] 
        public string Text { get; set; }

        [JsonProperty("image")] 
        public string Image { get; set; }
        
        [JsonProperty("likes")] 
        public int Likes { get; set; }

        [JsonProperty("tags")] 
        public List<string> Tags { get; set; }
        
        [JsonProperty("owner")] 
        public string OwnerId { get; set; }

        public override string ToString()
        {
            return $@"{Text}
{String.Join(' ', Tags)}
Likes: {Likes}";
        }
    }
}