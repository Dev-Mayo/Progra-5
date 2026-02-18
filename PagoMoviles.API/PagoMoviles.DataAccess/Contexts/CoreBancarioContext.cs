using Microsoft.EntityFrameworkCore;
using PagoMoviles.Entities;


namespace PagoMoviles.DataAccess.Contexts
{
    public class CoreBancarioContext : DbContext
    {
        public CoreBancarioContext(DbContextOptions<CoreBancarioContext> options)
            : base(options)
        {
        }

        
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Monedero> MonederosMoviles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}