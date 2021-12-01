using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Data;
using System.Threading.Tasks;

namespace SmoothBoardStylersApp.Controllers
{
    public class VragenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VragenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vragens
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vragen.Include(v => v.Vraagsteller);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vragens/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var vraag = await _context.Vragen.FindAsync(id);
            _context.Vragen.Remove(vraag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
