using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CleanCode.VariableNames3.Entities
{
    [Serializable]
    public class Student
    {
        [XmlIgnore]
        [JsonIgnore]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public HashSet<Lecture> Attendance { get; set; }
    }
}