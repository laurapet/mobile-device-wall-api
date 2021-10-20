using System;
namespace device_wall_backend.Modules.Dashboard.Control.DTOs
{
    public class DeviceDashboardDTO
    {
        public int DeviceID;
        public string Name;
        public string OperatingSystem;
        public string Version;
        public bool IsTablet;
        public int HorizontalSize;
        public int VerticalSize;
        public bool HasSIM;
        public CurrentLendDTO CurrentLendDTO;

        public DeviceDashboardDTO()
        {
        }
    }
}
