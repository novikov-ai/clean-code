using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace CleanCode.VariableNames3.Entities
{
    [Serializable]
    public class Lecturer
    {
        [XmlIgnore]
        [JsonIgnore]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}