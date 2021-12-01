using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothBoardStylersApp.Components
{
    public class FilialenViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public FilialenViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filialen = await _context.Filialen.ToListAsync();
            return View(filialen);
        }
    }
}
