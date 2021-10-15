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
            dashboardDTO.OperatingSystem = device.Features.OperatingSystem;
            dashboardDTO.Version = device.Features.Version;
            dashboardDTO.IsTablet = device.Features.IsTablet;
            dashboardDTO.HorizontalSize = device.Features.HorizontalSize;
            dashboardDTO.VerticalSize = device.Features.VerticalSize;
            dashboardDTO.HasSIM = device.Features.HasSIM;
            return dashboardDTO;
        }
    }
}
