using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmoothBoardStylersApp.Models
{
    public class Surfboard
    {
        public Surfboard()
        {
            InteresseContacten = new HashSet<Contact>();
            Voorraad = new HashSet<Voorraad>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(80)]
        public string Naam { get; set; }

        public string Beschrijving { get; set; }

        [ForeignKey("Materiaal")]
        [Display(Name = "Materiaal")]
        public int MateriaalId { get; set; }

        public virtual Materiaal Materiaal { get; set; }

        [DataType(DataType.Currency)]
        public decimal Prijs { get; set; }

        [StringLength(128)]
        [Display(Name ="Afbeelding")]
        public string FotoUrl { get; set; }

        public virtual ICollection<Contact> InteresseContacten { get; set; }

        public virtual ICollection<Voorraad> Voorraad { get; set; }
    }
}