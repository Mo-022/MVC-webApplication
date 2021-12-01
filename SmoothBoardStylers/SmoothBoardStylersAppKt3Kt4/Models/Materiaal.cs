using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmoothBoardStylersApp.Models
{
    public class Materiaal
    {
        public Materiaal()
        {
            Surfboards = new HashSet<Surfboard>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(80)]
        public string Naam { get; set; }

        public virtual ICollection<Surfboard> Surfboards { get; set; }
    }
}