using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using SalesWebMvc.Models;
using System.Diagnostics;

namespace SalesWebMvc.Data
{
    public class SalesWebMvcContext : DbContext
    {
        public SalesWebMvcContext(DbContextOptions<SalesWebMvcContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<SalesRecord> SalesRecord { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
            .HasMany(p => p.Sellers)
            .WithOne(t => t.Department)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Seller>()
            .HasMany(p => p.Sales)
            .WithOne(t => t.Seller)
            .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
