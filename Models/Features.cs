using System;
using System.Collections.Generic;

namespace device_wall_backend.Modules.Devices.Entity
{
    public class Features
    {
        public string OperatingSystem { get; set; }
        public string Version { get; set; }
        public string Manufacturer { get; set; }
        public bool IsTablet { get; set; }
        public int HorizontalSize { get; set; }
        public int VerticalSize { get; set; }
        public int Dpi { get; set; }
        public string Port { get; set; }
        public bool HasSIM { get; set; }
        public IEnumerable<string> exceptionalCases { get; set; }
    }
}
