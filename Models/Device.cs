using device_wall_backend.Modules.Devices.Entity;

namespace device_wall_backend.Models{
    public class Device{
        public int DeviceID {get; set;}
        public string Name {get; set;}
        
        //public int FeaturesID { get; set; }
        //public Features Features { get; set; }
        
        public string OperatingSystem { get; set; }
        public string Version { get; set; }
        public string Manufacturer { get; set; }
        public bool IsTablet { get; set; }
        public int HorizontalSize { get; set; }
        public int VerticalSize { get; set; }
        public int Dpi { get; set; }
        public string Port { get; set; }
        public bool HasSIM { get; set; }

    }
}