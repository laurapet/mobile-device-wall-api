using System;
using System.Collections.Generic;
using System.Net.Http;
using device_wall_backend.Data;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace device_wall_backend.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        public IntegrationTest()
        {
            var factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DeviceWallContext));
                        services.AddDbContext<DeviceWallContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDB");
                        });
                        
                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<DeviceWallContext>();
                            
                            db.Database.EnsureCreated();
                            InitializeDbForTests(db);
                        }
                    });
                });
            TestClient = factory.CreateClient();
        }
        
        public static void InitializeDbForTests(DeviceWallContext db)
        {
            db.Devices.AddRange(GetSeedingDevices());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(DeviceWallContext db)
        {
            db.Devices.RemoveRange(db.Devices);
            InitializeDbForTests(db);
        }

        public static List<Device> GetSeedingDevices()
        {
            return new List<Device>()
            {
                new Device {Name = "iPhone 5", OperatingSystem = "iOS", Version = "10.3.4"},
                new Device {Name = "Samsung Galaxy S5", OperatingSystem = "Android", Version = "6.0.1"},
                new Device {Name = "iPhone 5", OperatingSystem = "iOS", Version = "10.3.4", IsTablet = true},
                new Device {Name = "Samsung Galaxy S5", OperatingSystem = "Android", Version = "6.0.1", IsTablet = true}
            };
        }

    }
}