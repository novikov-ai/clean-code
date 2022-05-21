using System;
using System.Collections.Generic;

namespace CleanCode.CommentsClassification.Monitor
{
    public class DisplayedEntity
    {
        public string Name { get; set; }
        public string Panel { get; set; }
        public string Version { get; set; }
        public int Launches { get; set; }

        public int Users => UsersList.Count;
        
        public readonly HashSet<string> UsersList = new HashSet<string>();
        public int Errors { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null || !(this.GetType() == obj.GetType()))
                return false;
            
            var entity = obj as DisplayedEntity;
            if (entity is null)
                throw new ArgumentException($"Provide {entity.GetType().Name}");
                
            return entity.Name == this.Name && entity.Version == this.Version;
        }

        public override int GetHashCode()
        {
            return $"{Name}: {Version}".GetHashCode();
        }
    }
}