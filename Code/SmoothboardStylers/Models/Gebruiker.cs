using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Smoothboardstylers.Models
{
    public class Gebruiker : IdentityUser
    {
        [StringLength(80)]
        public string Naam { get; set; }
    }
}
