using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Smoothboardstylers.Data;
using Smoothboardstylers.Models;

namespace Smoothboardstylers.Controllers
{
    public class FiliaalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FiliaalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Filiaal
        public async Task<IActionResult> Index()
        {
            return View(await _context.Filialen.ToListAsync());
        }

        // GET: Filiaal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filiaal = await _context.Filialen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filiaal == null)
            {
                return NotFound();
            }

            return View(filiaal);
        }

        // GET: Filiaal/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Filiaal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,Plaats")] Filiaal filiaal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filiaal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(filiaal);
        }

        // GET: Filiaal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filiaal = await _context.Filialen.FindAsync(id);
            if (filiaal == null)
            {
                return NotFound();
            }
            return View(filiaal);
        }

        // POST: Filiaal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,Plaats")] Filiaal filiaal)
        {
            if (id != filiaal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filiaal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FiliaalExists(filiaal.Id))
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
            return View(filiaal);
        }

        // GET: Filiaal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filiaal = await _context.Filialen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filiaal == null)
            {
                return NotFound();
            }

            return View(filiaal);
        }

        // POST: Filiaal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filiaal = await _context.Filialen.FindAsync(id);
            _context.Filialen.Remove(filiaal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FiliaalExists(int id)
        {
            return _context.Filialen.Any(e => e.Id == id);
        }
        [HttpGet]
        public async Task<IActionResult> AddSurfboard(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            var filiaal = await _context.Filialen.FirstOrDefaultAsync(m => m.Id == id);
            if (filiaal == null)
            {
                return NotFound();
            }



            ViewData["SurfboardId"] = new SelectList(_context.Surfboards, "Id", "Naam");



            var voorraad = new Voorraad { FiliaalId = filiaal.Id };



            return View(voorraad);

        }

        [HttpPost]
        public async Task<IActionResult> AddSurfboard(int id, Voorraad voorraad)
        {
            if (id != voorraad.FiliaalId)
            {
                return NotFound();
            }



            var bestaandeVoorraad = await _context.Voorraad.FindAsync(voorraad.SurfboardId, voorraad.FiliaalId);
            if (bestaandeVoorraad == null)
            {
                _context.Add(voorraad);
            }
            else
            {
                bestaandeVoorraad.Aantal += voorraad.Aantal;
                _context.Update(bestaandeVoorraad);
            }



            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = voorraad.FiliaalId });
        }
    }
}
