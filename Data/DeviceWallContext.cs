using device_wall_backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Data
{
    public class DeviceWallContext : IdentityDbContext{

        public DeviceWallContext(DbContextOptions<DeviceWallContext> options) : base(options)
        {
        }

        public DbSet<Lending> Lendings { get; set; }
        public DbSet<Device> Devices { get; set; } 
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lending>().ToTable("Lending");
            modelBuilder.Entity<Device>().ToTable("Device");
            modelBuilder.Entity<User>().ToTable("User");
            base.OnModelCreating(modelBuilder);
        }
    }
}