using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace CleanCode.VariableNames3.Entities
{
    [Serializable]
    public class Discipline
    {
        [XmlIgnore]
        [JsonIgnore]
        public string Id { get; set; }
        public string Name { get; set; }
        public Lecturer Lecturer { get; set; }
    }
}