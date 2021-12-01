using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SmoothBoardStylersApp.Models
{
    public class Gebruiker : IdentityUser
    {
        [StringLength(80)]
        public string Naam { get; set; }
    }
}
