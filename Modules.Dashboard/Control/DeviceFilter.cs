using System;
namespace device_wall_backend.Modules.Dashboard.Control
{
    public class DeviceFilter
    {
        public string? OperatingSystem { get; set; }
        public string? Version { get; set; }
        public bool? IsTablet { get; set; }
        public bool? IsLent { get; set; }

    }
}
