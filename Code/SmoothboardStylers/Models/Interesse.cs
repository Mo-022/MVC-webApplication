using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Smoothboardstylers.Models
{
    public class Interesse
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public int SurfboardId { get; set; }
        public Surfboards Surfboard { get; set; }

        [DefaultValue(false)]
        public bool Bahandeld { get; set; }


    }
}
