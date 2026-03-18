using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoWebAPI.DataAccess.Models;


namespace ProyectoWebAPI.DataAccess.Models
{
    public class RolesContext : DbContext
    {
        public RolesContext(DbContextOptions<RolesContext> options)
            : base(options) { }
        public DbSet<Roles> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().HasKey(r => r.ID_Rol);
        }
    }
}
