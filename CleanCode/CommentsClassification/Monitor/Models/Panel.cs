using System.ComponentModel.DataAnnotations;

namespace CleanCode.CommentsClassification.Monitor.Models
{
    public partial class Panel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
