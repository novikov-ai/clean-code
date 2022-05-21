using System.ComponentModel.DataAnnotations;

namespace CleanCode.CommentsClassification.Monitor.Models
{
    public partial class Plugin
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Version { get; set; }

        public int? Panel_Id { get; set; }
    }
}
