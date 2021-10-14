using System;
using System.Linq;

namespace device_wall_backend.Gateway
{
    public class DbInitializer
    {
        public DbInitializer()
        {
        }

        public static void Initialize(DeviceWallContext context)
        {
            context.Database.EnsureCreated();

            if (context.Lendings.Any())
            {
                Console.WriteLine("LENDING TABLE EXISTS");
                return;   
            }


        }
    }
}
