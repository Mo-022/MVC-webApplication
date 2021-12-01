using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmoothBoardStylersApp.Models;

namespace SmoothBoardStylersApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<Gebruiker>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacten { get; set; }

        public DbSet<Surfboard> Surfboards { get; set; }

        public DbSet<Voorraad> Voorraad { get; set; }

        public DbSet<Filiaal> Filialen { get; set; }

        public DbSet<Materiaal> Materialen { get; set; }

        public DbSet<Faq> Faqs { get; set; }

        public DbSet<Vraag> Vragen { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Voorraad>()
                .HasKey(v => new { v.SurfboardId, v.FiliaalId });

            builder.Entity<Voorraad>()
                .HasOne(v => v.Surfboard)
                .WithMany(s => s.Voorraad)
                .HasForeignKey(v => v.SurfboardId);

            builder.Entity<Voorraad>()
                .HasOne(v => v.Filiaal)
                .WithMany(f => f.Voorraad)
                .HasForeignKey(v => v.FiliaalId);

            builder.Entity<Surfboard>()
                .Property(s => s.Prijs)
                .HasPrecision(18, 2);
        }
    }
}
