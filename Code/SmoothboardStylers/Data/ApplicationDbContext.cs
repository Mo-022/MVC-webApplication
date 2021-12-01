using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smoothboardstylers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smoothboardstylers.Data
{
    public class ApplicationDbContext : IdentityDbContext<Gebruiker>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Smoothboardstylers.Models.Surfboards> Surfboards { get; set; }
        public DbSet<Smoothboardstylers.Models.Materiaal> Materiaal { get; set; }
        public DbSet<Smoothboardstylers.Models.Contact> Contact { get; set; }
        public DbSet<Smoothboardstylers.Models.Interesse> Interesse { get; set; }
        public DbSet<Smoothboardstylers.Models.Voorraad> Voorraad { get; set; }
        public DbSet<Smoothboardstylers.Models.Filiaal> Filialen { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Voorraad>()
                .HasKey(v => new { v.SurfboardId, v.FiliaalId });
        }

    }
    
}
