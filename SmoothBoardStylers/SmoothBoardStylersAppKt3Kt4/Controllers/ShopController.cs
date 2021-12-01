using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Data;
using SmoothBoardStylersApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothBoardStylersApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var surfboards = await _context.Surfboards
                .Include(s => s.Materiaal)
                .OrderBy(s => s.Naam)
                .ToListAsync(); ;
            return View(surfboards);
        }

        public async Task<IActionResult> Board(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surfboard = await _context.Surfboards
                .Include(s => s.Materiaal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (surfboard == null)
            {
                return NotFound();
            }

            return View(surfboard);
        }

        [HttpPost]
        public async Task<IActionResult> Interesse(int? id, string email)
        {

            if (id == null)
            {
                return NotFound();
            }

            var surfboard = await _context.Surfboards
                .FindAsync(id);

            if (surfboard == null)
            {
                return NotFound();
            }

            if (! string.IsNullOrEmpty(email))
            {
                email = email.ToLower();
                var contact = await _context.Contacten
                    .FirstOrDefaultAsync(c => c.EmailAdres == email);

                if (contact == null)
                {
                    contact = new Contact
                    {
                        EmailAdres = email
                    };
                    _context.Add(contact);
                    await _context.SaveChangesAsync();
                }
                
                await _context.Entry(contact).Collection(c => c.SurfBoardInteresses).LoadAsync();

                if (contact.SurfBoardInteresses.FirstOrDefault(s => s.Id == id) == null)
                {
                    contact.SurfBoardInteresses.Add(surfboard);
                    try
                    {
                        _context.Update(contact);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                        return NotFound();
                    }
                }
                ViewData["Message"] = $"Dank je wel voor je interesse in {surfboard.Naam}";

            }

            return View("Board", surfboard);
        }
    }
}
