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
            // Roles iniciales
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Usuario" }
            );

            // Usuario Admin inicial con hash fijo de la contraseña "Jim1234!"
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Email = "admin@test.com",
                    Password = "$2b$12$eE6IjkEW8vm8fQnUuSA8EeibjU6OiLQaqGXX3wjE4tlYWl6/Ti2Uq", // hash fijo
                    EmailConfirmed = true,
                    RoleId = 1
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}

