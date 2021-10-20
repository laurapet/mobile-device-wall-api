using System;
using device_wall_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Modules.Dashboard.Gateway
{
    public class DashboardContext: DbContext
    {
        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
        {
            Console.WriteLine("context ctor");
        }

       // public DbSet<Lending> Lendings { get; set; }
        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<Lending>().ToTable("Lending");
            modelBuilder.Entity<Device>().ToTable("Device");
        }
    }
}