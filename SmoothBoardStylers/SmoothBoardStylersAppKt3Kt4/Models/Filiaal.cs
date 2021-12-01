using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmoothBoardStylersApp.Models
{
    public class Filiaal
    {
        [Key]
        public int Id { get; set; }

        [StringLength(80)]
        public string Naam { get; set; }

        [StringLength(120)]
        public string Adres { get; set; }

        [StringLength(120)]
        public string Woonplaats { get; set; }

        public virtual ICollection<Voorraad> Voorraad { get; set; }
    }
}