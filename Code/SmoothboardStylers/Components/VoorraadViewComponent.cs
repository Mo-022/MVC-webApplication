using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smoothboardstylers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smoothboardstylers.Components
{
    public class VoorraadViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public VoorraadViewComponent(ApplicationDbContext context)

        {

            _context = context;

        }
        public async Task<IViewComponentResult> InvokeAsync(int? id)

        {

            var voorraad = await _context.Voorraad

              .Include(v => v.Surfboard)

              .Include(v => v.Filiaal)

              .Where(v => v.FiliaalId == id)

              .ToListAsync();



            return View(voorraad);

        }


    }

}
