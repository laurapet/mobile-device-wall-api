using System;
using device_wall_backend.Modules.Dashboard.Control.DTOs;
using device_wall_backend.Models;

namespace device_wall_backend.Modules.Dashboard.Control
{
    public class DashboardDTOAdapter
    {
        public DashboardDTOAdapter()
        {
        }

        public DeviceDashboardDTO convertDeviceToDashboardDTO(Device device)
        {
            DeviceDashboardDTO dashboardDTO = new DeviceDashboardDTO();

            dashboardDTO.DeviceID = device.DeviceID;
            dashboardDTO.Name = device.Name;
            dashboardDTO.OperatingSystem = device.OperatingSystem;
            dashboardDTO.Version = device.Version;
            dashboardDTO.IsTablet = device.IsTablet;
            dashboardDTO.HorizontalSize = device.HorizontalSize;
            dashboardDTO.VerticalSize = device.VerticalSize;
            dashboardDTO.HasSIM = device.HasSIM;
            return dashboardDTO;
        }
    }
}
