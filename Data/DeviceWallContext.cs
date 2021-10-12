using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Data
{
    public class DeviceWallContext : DbContext{

        public DeviceWallContext(DbContextOptions<DeviceWallContext> options) : base(options)
        {
            Console.WriteLine("context ctor");
        }

        public DbSet<Lending> Lendings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lending>().ToTable("Lending");
        }
    }
}