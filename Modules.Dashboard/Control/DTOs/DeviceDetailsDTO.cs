using System;
using System.Collections.Generic;

namespace device_wall_backend.Modules.Dashboard.Control.DTOs
{
    public class DeviceDetailsDTO
    {
        public int DeviceID { get; set; }
        public string Name{ get; set; }
        public string OperatingSystem{ get; set; }
        public string Version{ get; set; }
        public bool IsTablet{ get; set; }
        public int HorizontalSize{ get; set; }
        public int VerticalSize{ get; set; }
        public bool HasSIM{ get; set; }
        public int Dpi { get; set; }
        public string Port { get; set; }
        public IEnumerable<String> ExceptionalCases { get; set; }

    }
}