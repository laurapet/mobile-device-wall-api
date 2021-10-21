using System;
using System.Collections.Generic;
using device_wall_backend.Modules.Devices.Entity;

//Features vielleicht noch als eigene Relation
namespace device_wall_backend.Models{
    public class Device{
        public int DeviceID {get; set;}
        public string Name {get; set;}
        public string OperatingSystem { get; set; }
        public string Version { get; set; }
        public string Manufacturer { get; set; }
        public bool IsTablet { get; set; }
        public int HorizontalSize { get; set; }
        public int VerticalSize { get; set; }
        public int Dpi { get; set; }
        public string Port { get; set; }
        public bool HasSIM { get; set; }
        
        public List<String> ExceptionalCases { get; set; }

        public Lending CurrentLending { get; set; }

    }
}