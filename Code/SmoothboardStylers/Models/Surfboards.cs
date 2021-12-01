using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smoothboardstylers.Models
{
    public class Surfboards
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Prijs { get; set; }
        public string FotoUrl { get; set; }
        public int MateriaalId { get; set; }
        public Materiaal Materiaal { get; set; }


    }
}
