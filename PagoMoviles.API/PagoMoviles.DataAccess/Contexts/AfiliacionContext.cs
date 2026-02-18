using Microsoft.EntityFrameworkCore;
using PagoMoviles.Entities;

namespace PagoMoviles.DataAccess.Contexts
{
    public class AfiliacionContext : DbContext
    {
        public AfiliacionContext(DbContextOptions<AfiliacionContext> options) : base(options) { }

        public DbSet<Monedero> Monedero { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Monedero>()
                .ToTable("monedero_movil", "dbo"); 
        }
    }
}




