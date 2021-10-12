using System;
using System.Linq;

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

            // Look for any students.
            if (context.Lendings.Any())
            {
                return;   // DB has been seeded
            }

            
        }
    }
}
