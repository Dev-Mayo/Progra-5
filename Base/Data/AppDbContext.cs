using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Esto representa la tabla en la base de datos
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Le decimos que la tabla se llama "Cliente" y la llave es IdUsuario
            modelBuilder.Entity<Cliente>().ToTable("Cliente").HasKey(u => u.Identificacion);
        }
    }
}
