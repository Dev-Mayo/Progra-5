using Microsoft.EntityFrameworkCore;
using PagoMoviles.Models.Entities;

namespace PagoMoviles.Data
{
    /// <summary>
    /// Contexto de base de datos para el sistema de Pagos Móviles.
    /// Maneja la tabla monedero_movil (inscripciones de pagos móviles).
    /// </summary>
    public class PagosMovilesDbContext : DbContext
    {
        public PagosMovilesDbContext(DbContextOptions<PagosMovilesDbContext> options)
            : base(options) { }

        public DbSet<MonederoMovil> MonederosMoviles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índice único: un teléfono solo puede estar asociado a una cuenta activa
            modelBuilder.Entity<MonederoMovil>(entity =>
            {
                entity.ToTable("monedero_movil");

                entity.HasIndex(e => e.NumeroTelefono)
                      .HasDatabaseName("IX_monedero_telefono");

                entity.HasIndex(e => e.Identificacion)
                      .HasDatabaseName("IX_monedero_identificacion");
            });
        }
    }
}
