using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothBoardStylersApp.Components
{
    public class VoorraadViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public VoorraadViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Toont de voorraad van Surfboards in een Filiaal
        /// </summary>
        /// <param name="id">het Id van het Filiaal</param>
        /// <returns>ViewComponentView met een overzicht van de voorraad</returns>
        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            var surfboards = await _context.Voorraad
                .Include(v => v.Filiaal)
                .Include(v => v.Surfboard)
                .Where(v => v.Aantal > 0)
                .OrderBy(v => v.Surfboard.Naam)
                .ToListAsync();

            if (id != null && id != 0)
            {
                var filiaal = await _context.Filialen.FindAsync(id);
                if (filiaal != null)
                {
                    surfboards = surfboards
                        .Where(v => v.FiliaalId == id)
                        .ToList();
                }
            }
            return View(surfboards);
        }
    }
}
