using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Smoothboardstylers.Data;
using Smoothboardstylers.Models;

namespace Smoothboardstylers.Controllers
{
    [Authorize]
    public class SurfboardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SurfboardsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Surfboards
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Surfboards.Include(s => s.Materiaal);
            return View(await applicationDbContext.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> Overzicht()
        {
            var applicationDbContext = _context.Surfboards.Include(s => s.Materiaal);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Surfboards/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Surfboards/Create
        public IActionResult Create()
        {
            ViewData["MateriaalId"] = new SelectList(_context.Set<Materiaal>(), "Id", "Naam");
            return View();
        }

        // POST: Surfboards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,Beschrijving,Prijs,FotoUrl,MateriaalId")] Surfboards surfboard, IFormFile fotoUrl)
        {
            if (ModelState.IsValid)
            {
                if (fotoUrl != null && fotoUrl.Length > 0)
                {
                    surfboard.FotoUrl = await SaveImage(fotoUrl);
                }

                _context.Add(surfboard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MateriaalId"] = new SelectList(_context.Set<Materiaal>(), "Id", "Naam", surfboard.MateriaalId);
            return View(surfboard);
        }

        private async Task<string> SaveImage(IFormFile fotoUrl)
        {
            string bestandsNaam = Path.GetFileName(fotoUrl.FileName)
                .Replace(' ', '-');
            int nummer = 0;

            string naamZonderEtensie = Path.GetFileNameWithoutExtension(bestandsNaam);
            string extensie = Path.GetExtension(bestandsNaam);

            string afbeeldingNaam = bestandsNaam;
            string afbeeldingPad;
            do
            {
                if (nummer > 0)
                {
                    afbeeldingNaam = $"{naamZonderEtensie}({nummer}){extensie}";
                }
                string imgPad = $"{_environment.WebRootPath}/img";
                afbeeldingPad = System.IO.Path.Combine(imgPad, afbeeldingNaam);
                nummer++;
            } while (System.IO.File.Exists(afbeeldingPad));
            try
            {
                using var stream = new FileStream(afbeeldingPad, FileMode.Create);
                await fotoUrl.CopyToAsync(stream);
                return afbeeldingNaam;
            }
            catch
            {
                return string.Empty;
            }
        }

        // GET: Surfboards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surfboard = await _context.Surfboards.FindAsync(id);
            if (surfboard == null)
            {
                return NotFound();
            }
            ViewData["MateriaalId"] = new SelectList(_context.Materiaal, "Id", "Naam");
            return View(surfboard);
        }

        // POST: Surfboards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,Beschrijving,Prijs,FotoUrl,MateriaalId")] Surfboards surfboard, IFormFile fotoUrl)
        {
            if (id != surfboard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (fotoUrl != null && fotoUrl.Length > 0)
                {
                    surfboard.FotoUrl = await SaveImage(fotoUrl);
                }
                try
                {
                    _context.Update(surfboard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurfboardExists(surfboard.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MateriaalId"] = new SelectList(_context.Materiaal, "Id", "Naam", surfboard.MateriaalId);
            return View(surfboard);
        }

        // GET: Surfboards/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Surfboards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var surfboard = await _context.Surfboards.FindAsync(id);
            _ = DeleteImage(surfboard.FotoUrl);

            _context.Surfboards.Remove(surfboard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeleteImage(string afbeeldingNaam)
        {
            string imgPad = $"{_environment.WebRootPath}/img";
            string afbeeldingPad = System.IO.Path.Combine(imgPad, afbeeldingNaam);
            try
            {
                System.IO.File.Delete(afbeeldingPad);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool SurfboardExists(int id)
        {
            return _context.Surfboards.Any(e => e.Id == id);
        }
    }
}
