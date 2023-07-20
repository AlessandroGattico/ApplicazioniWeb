using System.ComponentModel.DataAnnotations;

namespace GestoreDBMS.Models
{
    public class SqLiteModel
    {
        // Proprietà "path": rappresenta il percorso del database SQLite
        [Required]
        [Key]
        public string path { get; set; }
    }
}
