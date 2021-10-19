using System;
using System.Linq;
using device_wall_backend.Modules.Dashboard.Gateway;
using device_wall_backend.Modules.Lendings.Gateway;

namespace device_wall_backend.Data
{
    public class DbInitializer
    {
        public DbInitializer()
        {
        }

        public static void Initialize(DeviceWallContext context)
        {
            context.Database.EnsureCreated();

        }
    }
}
