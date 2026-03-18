using Microsoft.EntityFrameworkCore;
using ProyectoWebAPI.DataAccess.Models;

namespace ProyectoWebAPI.DataAccess
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().HasKey(c => c.cliente_id);
        }
    }
}
