using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmoothBoardStylersApp.Models
{
    public class Vraag
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Vraagsteller")]
        public int VraagstellerId { get; set; }

        public string Tekst { get; set; }

        public virtual Contact Vraagsteller { get; set; }
    }
}
