using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothBoardStylersApp.Models
{
    public class Contact
    {
        public Contact()
        {
            SurfBoardInteresses = new HashSet<Surfboard>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "E-mailadres")]
        [DataType(DataType.EmailAddress)]
        public string EmailAdres { get; set; }

        [Display(Name = "Geïnteresseerd in")]
        public virtual ICollection<Surfboard> SurfBoardInteresses { get; set; }

        public virtual ICollection<Vraag> Vragen { get; set; }
    }
}
