using Microsoft.EntityFrameworkCore;
using PagoMoviles.Models.Entities;

namespace PagoMoviles.Data
{
    /// <summary>
    /// Contexto de base de datos del Core Bancario (solo lectura para nuestros servicios).
    /// Se usa en SRV19 para verificar si un cliente existe.
    /// Las tablas (cliente, cuenta) ya fueron creadas por otro compañero.
    /// </summary>
    public class CoreBancarioDbContext : DbContext
    {
        public CoreBancarioDbContext(DbContextOptions<CoreBancarioDbContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("cliente");
                entity.HasIndex(e => e.Identificacion).IsUnique();
            });

            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.ToTable("cuenta");
                entity.HasIndex(e => e.NumeroCuenta).IsUnique();
                entity.HasOne(e => e.Cliente)
                      .WithMany()
                      .HasForeignKey(e => e.ClienteId);
            });
        }
    }
}
