using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Data;
using SmoothBoardStylersApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SmoothBoardStylersApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var filialen = await _context.Filialen.ToListAsync();
            return View();
        }

        public IActionResult Shapes()
        {
            // Er moet nog een pagina komen over de shapes van surfboards
            // Hieronder een voorbeeld.
            // Shapes mag nog niet in het menu
            return Redirect("https://centerforsurfresearch.org/surfboard-shapes/");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
