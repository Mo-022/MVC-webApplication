using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Data;
using SmoothBoardStylersApp.Models;

namespace SmoothBoardStylersApp.Controllers
{
    [Authorize]
    public class VoorraadController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoorraadController(ApplicationDbContext context)
        {
            _context = context;
        }

        private Filiaal GeenFiliaal => new Filiaal { Id = 0, Naam = "Alle Filialen" };

        private async Task<List<Filiaal>> FilialenKeuzes()
        {
            var filialen = await _context.Filialen
                .OrderBy(f => f.Naam)
                .ToListAsync();

            filialen.Insert(0, GeenFiliaal);
            return filialen;
        }

        // GET: Voorraad
        public async Task<IActionResult> Index()
        {
            var filiaal = GeenFiliaal;

            var filialen = await FilialenKeuzes();
            ViewData["FiliaalId"] = new SelectList(filialen, "Id", "Naam");
            return View(filiaal);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int filiaal)
        {
            var selectie = await _context.Filialen.FindAsync(filiaal);
            if (selectie == null)
            {
                selectie = GeenFiliaal;
            }
            var filialen = await FilialenKeuzes();
            ViewData["FiliaalId"] = new SelectList(filialen, "Id", "Naam", selectie.Id);
            return View(selectie);
        }

        private List<Surfboard> SurfboardsNietInVoorraad(int id)
        {
            var surfboards = _context.Surfboards
                .OrderBy(s => s.Naam)
                .ToList();

            if (id == 0)
            {
                return surfboards;
            }

            var surfboardsInVoorraad = _context.Voorraad
               .Include(v => v.Surfboard)
               .Where(v => v.FiliaalId == id && v.Aantal > 0)
               .Select(v => v.Surfboard)
               .ToList();

            surfboards = surfboards
                .Except(surfboardsInVoorraad)
                .ToList();

            return surfboards;
        }

        // GET: Voorraad/Create
        public async Task<IActionResult> Create(int? id)
        {
            var filiaal = await _context.Filialen.FindAsync(id);

            if (filiaal == null)
            {
                filiaal = new Filiaal { Id = 0 };
            }
            var surfboards = SurfboardsNietInVoorraad(filiaal.Id);

            var voorraad = new Voorraad { FiliaalId = filiaal.Id };
            ViewData["FiliaalId"] = new SelectList(_context.Filialen, "Id", "Naam", filiaal.Id);
            ViewData["SurfboardId"] = new SelectList(surfboards, "Id", "Naam");
            return View(voorraad);
        }

        // POST: Voorraad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SurfboardId,FiliaalId,Aantal")] Voorraad voorraad)
        {
            if (ModelState.IsValid)
            {

                var bestaandeVoorraad = await _context.Voorraad.FindAsync(voorraad.SurfboardId, voorraad.FiliaalId);
                if (bestaandeVoorraad == null)
                {
                    try
                    {
                        _context.Add(voorraad);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);                        
                    }
                }
                else
                {
                    bestaandeVoorraad.Aantal += voorraad.Aantal; 
                    _context.Update(bestaandeVoorraad);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index), new { Id = voorraad.FiliaalId });
            }

            var surfboards = SurfboardsNietInVoorraad(voorraad.FiliaalId);
            ViewData["FiliaalId"] = new SelectList(_context.Filialen, "Id", "Naam", voorraad.FiliaalId);
            ViewData["SurfboardId"] = new SelectList(surfboards, "Id", "Naam", voorraad.SurfboardId);
            return View(voorraad);
        }

        // GET: Voorraad/Edit/5
        public async Task<IActionResult> Edit(int surfboardId, int filiaalId)
        {
            var voorraad = await _context.Voorraad
                .Include(v => v.Filiaal)
                .Include(v => v.Surfboard)
                .FirstOrDefaultAsync(v => v.SurfboardId == surfboardId && v.FiliaalId == filiaalId);
            if (voorraad == null)
            {
                return NotFound();
            }
            return View(voorraad);
        }

        // POST: Voorraad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Voorraad voorraad)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voorraad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return RedirectToAction(nameof(Index), new { Id = voorraad.FiliaalId });
            }

            return View(voorraad);
        }

        // GET: Voorraad/Delete/5
        public async Task<IActionResult> Delete(int surfboardId, int filiaalId)
        {
            var voorraad = await _context.Voorraad.FindAsync(new { surfboardId, filiaalId });

            if (voorraad == null)
            {            
                return NotFound();
            }

            voorraad.Aantal = 0;
            try
            {
                _context.Update(voorraad);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(voorraad);
        }
    }
}
