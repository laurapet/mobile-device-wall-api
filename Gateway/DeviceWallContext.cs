using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using device_wall_backend.Models;

namespace device_wall_backend.Gateway
{
    public class DeviceWallContext : DbContext{

        public DeviceWallContext(DbContextOptions<DeviceWallContext> options) : base(options)
        {
        }

        public DbSet<Lending> Lendings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lending>().ToTable("Lending");
        }
    }
}