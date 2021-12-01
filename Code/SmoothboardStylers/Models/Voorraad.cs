using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smoothboardstylers.Models
{
    public class Voorraad
    {
        public int SurfboardId { get; set; }
        public Surfboards Surfboard { get; set; }
        public int FiliaalId { get; set; }
        public Filiaal Filiaal { get; set; }
        public int Aantal { get; set; }
    }
}
