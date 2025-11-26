using Microsoft.EntityFrameworkCore;
using TrabajoPracticoApp.Models;

namespace TrabajoPracticoApp.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
       
        public DbSet<Role> Roles { get; set; }
            public DbSet<User> Users { get; set; }

        public DbSet<Compra> Compras { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Usuario" }
                    );
                base.OnModelCreating(modelBuilder);
            }
        
    }
    
}
