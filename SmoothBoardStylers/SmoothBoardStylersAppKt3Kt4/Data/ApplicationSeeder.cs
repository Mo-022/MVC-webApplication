using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmoothBoardStylersApp.Models;

namespace SmoothBoardStylersApp.Data
{
    public class ApplicationSeeder
    {
        private const string _password = "Password123!";

        private List<string> _roleNames = new() { "Admin" };

        private Dictionary<string, string> _admins = new() 
        { 
            { "sean@smoothboardstylers.com", "Sean Jensen" },
            { "marian@smoothboardstylers.com", "Marian Jensen" }
        };

        private readonly UserManager<Gebruiker> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationSeeder(UserManager<Gebruiker> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected async Task SeedRoles()
        {
            foreach(var roleName in _roleNames)
            {
                var exists = await _roleManager.RoleExistsAsync(roleName);
                if(! exists )
                {
                    IdentityRole role = new IdentityRole { Name = roleName };
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        public async Task Seedusers()
        {
            await SeedRoles();

            foreach (KeyValuePair<string, string> beheerder in _admins)
            {
                var admin = new Gebruiker
                {
                    Email = beheerder.Key,
                    Naam = beheerder.Value,
                    EmailConfirmed = true,
                };
                var user = await _userManager.FindByEmailAsync(admin.Email);
                if (user == null)
                {
                    admin.UserName = admin.Email;
                    var result = await _userManager.CreateAsync(admin, _password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }
        }
    }
}
