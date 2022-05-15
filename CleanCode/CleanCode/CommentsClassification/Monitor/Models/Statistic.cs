using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanCode.CommentsClassification.Monitor.Models
{
    public partial class Statistic
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string User_Name { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Start_Date { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? End_Date { get; set; }

        [StringLength(1000)]
        public string Result { get; set; }

        public int Plugin_Id { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj is null || !(this.GetType() == obj.GetType()))
                return false;
            
            var entity = obj as Statistic;
            if (entity is null)
                throw new ArgumentException($"Provide {entity.GetType().Name}");
                
            return entity.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return $"{Id}: {Start_Date}-{End_Date}".GetHashCode();
        }
    }
}
