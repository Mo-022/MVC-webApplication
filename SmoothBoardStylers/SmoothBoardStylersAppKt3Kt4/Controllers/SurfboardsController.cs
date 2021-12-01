using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Data;
using SmoothBoardStylersApp.Models;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothBoardStylersApp.Controllers
{
    [Authorize]
    public class SurfboardsController : Controller
    {
        private const string _surfBoardIcon = "surfboard-icon.png";

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
            var surfboards = await _context.Surfboards
                .Include(s => s.Materiaal)
                .OrderBy(s => s.Naam)
                .ToListAsync();
            return View(surfboards);
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
            ViewData["MateriaalId"] = new SelectList(_context.Materialen, "Id", "Naam");
            return View();
        }

        // POST: Surfboards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,Beschrijving,MateriaalId,Prijs")] Surfboard surfboard, IFormFile fotoFile)
        {
            if (ModelState.IsValid)
            {
                if (fotoFile != null && fotoFile.Length >0)
                {
                    surfboard.FotoUrl = await SaveImage(fotoFile);
                }                
                if (string.IsNullOrEmpty(surfboard.FotoUrl))
                {
                    surfboard.FotoUrl = _surfBoardIcon;
                }
                _context.Add(surfboard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MateriaalId"] = new SelectList(_context.Materialen, "Id", "Naam", surfboard.MateriaalId);
            return View(surfboard);
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
            ViewData["MateriaalId"] = new SelectList(_context.Materialen, "Id", "Id", surfboard.MateriaalId);
            return View(surfboard);
        }

        // POST: Surfboards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,Beschrijving,MateriaalId,Prijs,FotoUrl")] Surfboard surfboard, IFormFile fotoFile)
        {
            if (id != surfboard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (fotoFile != null && fotoFile.Length > 0)
                {
                    surfboard.FotoUrl = await SaveImage(fotoFile);
                }
                if (string.IsNullOrEmpty(surfboard.FotoUrl))
                {
                    surfboard.FotoUrl = _surfBoardIcon;
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
            ViewData["MateriaalId"] = new SelectList(_context.Materialen, "Id", "Naam", surfboard.MateriaalId);
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
            _context.Surfboards.Remove(surfboard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SurfboardExists(int id)
        {
            return _context.Surfboards.Any(e => e.Id == id);
        }

        #region Helpers

        private async Task<string> SaveImage(IFormFile fotoFile)
        {
            var fileName = Path.GetFileName(fotoFile.FileName).Replace(' ', '-');

            int number = 0;

            var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);

            var savedName = fileName;
            string fotoPath;

            do
            {
                if (number > 0)
                {
                    savedName = $"{nameWithoutExtension}({number}){extension}";
                }
                fotoPath = ImagePath(savedName);
                number++;
            } while (System.IO.File.Exists(fotoPath));

            try
            {
                using var stream = new FileStream(fotoPath, FileMode.Create);
                await fotoFile.CopyToAsync(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

            if (!Thumbnail(savedName))
            {
                DeleteImage(savedName);
                return string.Empty;
            }

            return savedName;

        }

        private bool DeleteImage(string savedName)
        {
            try
            {
                string imagePath = ImagePath(savedName);
                System.IO.File.Delete(imagePath);

                string thumbnailPath = ImagePath($"thumb.{savedName}");
                System.IO.File.Delete(thumbnailPath);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        private bool Thumbnail(string savedName)
        {
            int thumbnailwidth = 160;
            var imagePath = ImagePath(savedName);

            try
            {
                var original = new Bitmap(imagePath);
                var myCallBack = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                float resizeFactor = (float)thumbnailwidth / (float)original.Width;
                int thumbnalHeight = (int)(resizeFactor * original.Height);

                var thumbnail = original.GetThumbnailImage(thumbnailwidth, thumbnalHeight, myCallBack, IntPtr.Zero);

                var thumbnailName = $"thumbnail.{savedName}";
                var thumbnailPath = ImagePath(thumbnailName);

                thumbnail.Save(thumbnailPath);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string ImagePath(string imageFileName)
        {
            string imgPad = $"{_environment.WebRootPath}/img";
            string imagePath = Path.Combine(imgPad, imageFileName);
            return imagePath;
        }

        #endregion
    }
}
