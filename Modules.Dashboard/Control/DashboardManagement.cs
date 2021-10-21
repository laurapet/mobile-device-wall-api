using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Modules.Dashboard.Control.DTOs;
using device_wall_backend.Modules.Dashboard.Gateway;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Dashboard.Control
{
    public class DashboardManagement: IDashboardManagement
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly DashboardDTOAdapter _converter;

        public DashboardManagement(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
            _converter = new DashboardDTOAdapter();
        }

        public async Task<IEnumerable<DeviceDashboardDTO>> GetDevicesForDashboard(DeviceFilter filter)
        {
            var devices = await _dashboardRepository.GetDevicesForDashboard(filter);
            List<DeviceDashboardDTO> deviceDashboardDTOs = new List<DeviceDashboardDTO>();
            foreach(Device d in devices)
            {
                deviceDashboardDTOs.Add(_converter.ConvertDeviceToDashboardDTO(d));
            }

            return deviceDashboardDTOs;

        }

        public Task<ActionResult<Device>> GetDeviceDetails(int deviceID)
        {
            var device = _dashboardRepository.GetDeviceDetails(deviceID);
            //TODO: vielleicht zum DTO konvertieren (bisher unterscheiden sich Device & DetailsDTO nicht)
            return device;
        }
    }
}
