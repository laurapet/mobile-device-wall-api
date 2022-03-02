using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using device_wall_backend.Data;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Authentication;
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

        private List<Lending> _seedingLendings
        {
            get
            {
                return new List<Lending>()
                {
                   // new() {DeviceID = 1, DeviceWallUser = new(){UserName = "test", Name = "testname", Id = 1}, IsLongterm = false},
                    new() {DeviceID = 3, DeviceWallUser = new(){UserName = "test", Name = "testname", Id = 3}, IsLongterm = false},
                    new() {DeviceID = 4, DeviceWallUser = new(){UserName = "test", Name = "testname", Id = 4}, IsLongterm = false},
                };
            }
        }
        
        private List<DeviceWallUser> _seedingUsers
        {
            get
            {
                return new List<DeviceWallUser>()
                {
                    new(){UserName = "test", Name = "testname", Id = 1},
                    new(){UserName = "test2", Name = "testname2", Id = 2}
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
                        
                        services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "Test", options => {});
                        
                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<DeviceWallContext>();
                            
                            if (!db.Devices.Any()) {
                                db.Database.EnsureDeleted();
                                db.Database.EnsureCreated();
                                InitializeDbForTests(db);
                            }/*
                            else
                            {
                              ReinitializeDbForTests(db);
                            }*/
                        }
                    });
                });
            TestClient = factory.CreateClient();
            
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
        }
        
        public void InitializeDbForTests(DeviceWallContext db)
        {
            db.Lendings.AddRange(_seedingLendings);
            db.Devices.AddRange(_seedingDevices);
            db.DeviceWallUsers.AddRange(_seedingUsers);
            
            db.SaveChanges();
        }

        public void ReinitializeDbForTests(DeviceWallContext db)
        {
            //db.Lendings.RemoveRange(db.Lendings);
            db.Devices.RemoveRange(db.Devices);
            db.DeviceWallUsers.RemoveRange(db.DeviceWallUsers);
            InitializeDbForTests(db);
        }
    }
}