using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothBoardStylersApp.Controllers
{
    public class ContactenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contacten
        public async Task<IActionResult> Index()
        {
            var contacten = await _context.Contacten
                .ToListAsync();

            foreach(var contact in contacten)
            {
                _context.Entry(contact).Collection(c => c.SurfBoardInteresses).Load();
            }
            return View(contacten);
        }

        public async Task<IActionResult> RemoveInteresse(int contactId, int surfboardId)
        {
            var contact = await _context.Contacten.FindAsync(contactId);
            _context.Entry(contact).Collection(c => c.SurfBoardInteresses).Load();

            var surfboard = contact.SurfBoardInteresses.FirstOrDefault(s => s.Id == surfboardId);
            contact.SurfBoardInteresses.Remove(surfboard);
            _context.Update(contact);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
