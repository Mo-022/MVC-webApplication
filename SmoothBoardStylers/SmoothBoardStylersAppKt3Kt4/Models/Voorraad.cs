using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmoothBoardStylersApp.Models
{
    public class Voorraad
    {
        [ForeignKey("Surfboard")]
        [Display(Name = "Surfboard")]
        public int SurfboardId { get; set; }

        public virtual Surfboard Surfboard { get; set; }

        [ForeignKey("Filiaal")]
        [Display(Name = "Filiaal")]
        public int FiliaalId { get; set; }

        public virtual Filiaal Filiaal { get; set;  }

        public int Aantal { get; set; }
    }
}