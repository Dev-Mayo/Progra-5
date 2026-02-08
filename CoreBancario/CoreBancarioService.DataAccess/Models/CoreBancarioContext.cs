using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoreBancarioService.DataAccess.Models;

public partial class CoreBancarioContext : DbContext
{
    public CoreBancarioContext()
    {
    }

    public CoreBancarioContext(DbContextOptions<CoreBancarioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<Cuenta> Cuenta { get; set; }

    public virtual DbSet<Movimiento> Movimiento { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback solo si no se configuró vía DI
            optionsBuilder.UseSqlServer("Server=PC-DIEGO\\SQLINSTADEV1;Database=CoreBancario;User id=sa;password=SQLcontra;Encrypt=False;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__cliente__47E34D6483FA71B0");

            entity.ToTable("cliente");

            entity.HasIndex(e => e.Identificacion, "UQ__cliente__C196DEC703E83077").IsUnique();

            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.ContrasenaHash).HasMaxLength(64);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Identificacion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("identificacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoIdentificacion).HasColumnName("Tipo_Identificacion");
        });

        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.HasKey(e => e.CuentaId).HasName("PK__cuenta__612B08617D31F8E6");

            entity.ToTable("cuenta");

            entity.HasIndex(e => e.NumeroCuenta, "UQ__cuenta__C6B74B8883245E0F").IsUnique();

            entity.Property(e => e.CuentaId).HasColumnName("cuenta_id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.NumeroCuenta)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numero_cuenta");
            entity.Property(e => e.Saldo)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("saldo");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cuenta_cliente");
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => e.MovimientoId).HasName("PK__movimien__A87EF0E5A13373B3");

            entity.ToTable("movimiento");

            entity.Property(e => e.MovimientoId).HasColumnName("movimiento_id");
            entity.Property(e => e.CuentaId).HasColumnName("cuenta_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_movimiento");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.SaldoActual)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("saldo_actual");
            entity.Property(e => e.SaldoAnterior)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("saldo_anterior");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_movimiento");

            entity.HasOne(d => d.Cuenta).WithMany(p => p.Movimiento)
                .HasForeignKey(d => d.CuentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_movimiento_cuenta");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
