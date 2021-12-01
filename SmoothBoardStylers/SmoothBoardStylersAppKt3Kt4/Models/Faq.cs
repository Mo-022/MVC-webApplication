using System.ComponentModel.DataAnnotations;

namespace SmoothBoardStylersApp.Models
{
    public class Faq
    {
        [Key]
        public int Id { get; set; }

        [StringLength(120)]
        public string Vraag { get; set; }

        [DataType(DataType.MultilineText)]
        public string Antwoord { get; set; }
    }
}
