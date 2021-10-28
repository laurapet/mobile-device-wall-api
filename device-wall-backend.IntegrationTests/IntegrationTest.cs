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

        private List<Device> _seedingDevices
        {
            get
            {
                return new List<Device>()
                {
                    new() {Name = "iPhone 5", OperatingSystem = "iOS", Version = "10.3.4"},
                    new() {Name = "Samsung Galaxy S5", OperatingSystem = "Android", Version = "6.0.1"},
                    new() {Name = "iPhone 5", OperatingSystem = "iOS", Version = "10.3.4", IsTablet = true},
                    new() {Name = "Samsung Galaxy S5", OperatingSystem = "Android", Version = "6.0.1", IsTablet = true}
                };
            }
        }
        
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
        
        public void InitializeDbForTests(DeviceWallContext db)
        {
            db.Devices.AddRange(_seedingDevices);
            db.SaveChanges();
        }

        public void ReinitializeDbForTests(DeviceWallContext db)
        {
            db.Devices.RemoveRange(db.Devices);
            InitializeDbForTests(db);
        }
    }
}