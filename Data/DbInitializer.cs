using System;
using System.Linq;
using device_wall_backend.Models;
using device_wall_backend.Modules.Dashboard.Gateway;

namespace device_wall_backend.Data
{
    public class DbInitializer
    {
        public DbInitializer()
        {
        }

        public static void Initialize(DeviceWallContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Devices.Any())
            {
                
                var devices = new Device[]
                {
                    new Device {Name = "iPhone 5", OperatingSystem = "iOS", Version = "10.3.4", HorizontalSize = 1170, VerticalSize = 2532, Dpi = 460, Manufacturer = "Apple", Port = "Lightning", IsTablet = false, ExceptionalCases = "Gerät befindet sich im iOS Beta-Programm"},
                    new Device {Name = "iPhone 6", OperatingSystem = "iOS", Version = "10.3.4"},
                    new Device {Name = "iPhone 7", OperatingSystem = "iOS", Version = "10.3.4"},
                    new Device {Name = "iPhone 8", OperatingSystem = "iOS", Version = "10.3.4"},
                    new Device {Name = "Samsung Galaxy S5", OperatingSystem = "Android", Version = "6.0.1"},
                    new Device {Name = "Samsung Galaxy S6", OperatingSystem = "Android", Version = "6.0.1"},
                };
                foreach (Device d in devices)
                {
                    context.Devices.Add(d);
                }

                context.SaveChanges();
            }

            if (!context.Lendings.Any())
            {
                var lendings = new Lending[]
                {
                    /*new Lending
                    {
                        DeviceID = 3, IsLongterm = true, Device = new Device {Name = "iPhone 5"},
                        //User = new User {Username = "u1"}
                    },
                    new Lending
                    {
                        DeviceID = 2, IsLongterm = true, Device = new Device {Name = "Samsung Galaxy S5"},
                        //User = new User {Username = "u2"}
                    }*/
                };
            
                foreach (Lending l in lendings)
                {
                    context.Lendings.Add(l);
                }
                context.SaveChanges();
            }
        }
    }
}
