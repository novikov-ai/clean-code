using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace CleanCode.VariableNames3.Entities
{
    [Serializable]
    public class Lecture
    {
        [XmlIgnore]
        [JsonIgnore]
        public string Id { get; set; }

        public Discipline Discipline { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public Homework Homework { get; set; }
        public bool Attended { get; set; }
        public DateTime Date { get; set; }
    }
}