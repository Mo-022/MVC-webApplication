using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Data;
using SmoothBoardStylersApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothBoardStylersApp.Controllers
{
    [Authorize]
    public class FaqsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FaqsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Faqs
        public async Task<IActionResult> Index()
        {
            var aantalVragen = _context.Vragen.Count();
            if (aantalVragen > 0)
            {
                ViewData["AantalVragen"] = $"{aantalVragen} vragen zijn nog niet beantwoord.";
            }
            return View(await _context.Faqs.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Faq()
        {
            return View(await _context.Faqs.ToListAsync());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Faq(string vraag, string email)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(vraag))
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

                var nieuweVraag = new Vraag { Tekst = vraag, VraagstellerId = contact.Id };
                _context.Add(nieuweVraag);
                await _context.SaveChangesAsync();
                ViewData["Message"] = "Dank je wel voor je vraag. We beantwoorden hem zo snel mogelijk.";
                return await Faq();
            }
            return RedirectToAction(nameof(Faq));
        }

        // GET: Faqs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faqs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Vraag,Antwoord")] Faq faq)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faq);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faq);
        }

        // GET: Faqs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        // POST: Faqs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vraag,Antwoord")] Faq faq)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faq);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaqExists(faq.Id))
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
            return View(faq);
        }

        // GET: Faqs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // POST: Faqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faq = await _context.Faqs.FindAsync(id);
            _context.Faqs.Remove(faq);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }        

        private bool FaqExists(int id)
        {
            return _context.Faqs.Any(e => e.Id == id);
        }
    }
}
