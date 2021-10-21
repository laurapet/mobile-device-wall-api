using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using device_wall_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Modules.Devices.Entity
{
    public class Features
    {
        public int FeaturesID { get; set; }
        public string OperatingSystem { get; set; }
        public string Version { get; set; }
        public string Manufacturer { get; set; }
        public bool IsTablet { get; set; }
        public int HorizontalSize { get; set; }
        public int VerticalSize { get; set; }
        public int Dpi { get; set; }
        public string Port { get; set; }
        public bool HasSIM { get; set; }
        public List<string> ExceptionalCases { get; set; }
        
    }
}
