namespace device_wall_backend.Modules.Dashboard.Control.DTOs
{
    public class DeviceDashboardDTO
    {
        public int DeviceID { get; set; }
        public string Name{ get; set; }
        public string OperatingSystem{ get; set; }
        public string Version{ get; set; }
        public bool IsTablet{ get; set; }
        public int HorizontalSize{ get; set; }
        public int VerticalSize{ get; set; }
        public bool HasSIM{ get; set; }
        public CurrentLendingDTO currentLending { get; set; }
        
        public string LinkToDetails { get; set; }
    }
}