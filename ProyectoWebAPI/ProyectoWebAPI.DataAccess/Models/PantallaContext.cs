using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoWebAPI.DataAccess.Models;

namespace ProyectoWebAPI.DataAccess.Models
{
    public class PantallaContext : DbContext
    {
        public PantallaContext(DbContextOptions<PantallaContext> options)
            : base(options) { }

        public DbSet<Pantalla> Pantallas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pantalla>().HasKey(c => c.ID_Pantalla);
        }
    }
}
