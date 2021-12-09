namespace device_wall_backend.Modules.Lendings.Control.DTOs
{
    public class OwnLendingDTO
    {
        public int DeviceID { get; set; }
        public string DeviceName{ get; set; }
        public string OperatingSystem{ get; set; }
        public string Version{ get; set; }
        public bool IsTablet{ get; set; }
        public int HorizontalSize{ get; set; }
        public int VerticalSize{ get; set; }
        public bool HasSIM{ get; set; }
        public bool IsLongterm { get; set; }
    }
}