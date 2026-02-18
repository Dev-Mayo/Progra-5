using Microsoft.EntityFrameworkCore;
using PagoMoviles.Entities;

namespace PagoMoviles.DataAccess.Contexts
{
    public class PagosMovilesContext : DbContext
    {
        public PagosMovilesContext(DbContextOptions<PagosMovilesContext> options)
            : base(options)
        {
        }

        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Le decimos que la clase "Transaccion" es la tabla PAM_TRANSACCION_TB
            modelBuilder.Entity<Transaccion>().ToTable("PAM_TRANSACCION_TB");

            // 2. Mapeamos los nombres de las columnas que puso David (los que tienen TRX_)
            modelBuilder.Entity<Transaccion>(entity =>
            {
                entity.HasKey(e => e.TrxId);
                entity.Property(e => e.TrxId).HasColumnName("TRX_ID");
                entity.Property(e => e.TrxFecha).HasColumnName("TRX_FECHA");
                entity.Property(e => e.TrxMonto).HasColumnName("TRX_MONTO");
                entity.Property(e => e.TrxEntOrigen).HasColumnName("TRX_ENT_ORIGEN");
                entity.Property(e => e.TrxEntDestino).HasColumnName("TRX_ENT_DESTINO");
                entity.Property(e => e.TrxEstado).HasColumnName("TRX_ESTADO");
            });
        }
    }
}