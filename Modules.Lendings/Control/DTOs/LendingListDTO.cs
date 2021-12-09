using System.Collections.Generic;

namespace device_wall_backend.Modules.Lendings.Control.DTOs
{
    //To collect multiple Devices to Lend at the same time
    public class LendingListDTO
    {
        public int DeviceID { get; set; }
        public bool IsLongterm { get; set; }
    }
}