using System;
namespace device_wall_backend.Modules.Dashboard.Control
{
    public class DeviceFilter
    {
        public string? operatingSystem { get; set; }
        public string? version { get; set; }
        public bool? isTablet { get; set; }
        public bool? isLent { get; set; }

    }
}
