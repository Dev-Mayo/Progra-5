using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoWebAPI.DataAccess.Models;


namespace ProyectoWebAPI.DataAccess.Models
{
    public class EntidadesBancariasContext : DbContext
    {
        public EntidadesBancariasContext(DbContextOptions<EntidadesBancariasContext> options)
            : base(options) { }
        public DbSet<EntidadesBancarias> EntidadesBancarias { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntidadesBancarias>().HasKey(e => e.ID_Entidad);
        }
    }
}
