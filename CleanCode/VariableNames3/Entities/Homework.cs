using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace CleanCode.VariableNames3.Entities
{
    [Serializable]
    public class Homework
    {
        [XmlIgnore]
        [JsonIgnore]
        public string Id { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public Lecturer Lecturer { get; set; }
        public Discipline Discipline { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public int Mark { get; set; }
    }
}