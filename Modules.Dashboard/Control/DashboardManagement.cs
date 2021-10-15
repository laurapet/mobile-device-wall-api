using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Modules.Dashboard.Control.DTOs;
using device_wall_backend.Modules.Dashboard.Gateway;
using device_wall_backend.Models;

namespace device_wall_backend.Modules.Dashboard.Control
{
    public class DashboardManagement: IDashboardManagement
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly DashboardDTOAdapter _converter;

        public DashboardManagement(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IEnumerable<DeviceDashboardDTO>> getDevicesForDashboard(DeviceFilter filter)
        {
            var devices = await _dashboardRepository.getDevicesForDashboard(filter);
            List<DeviceDashboardDTO> deviceDashboardDTOs = new List<DeviceDashboardDTO>();
            foreach(Device d in devices)
            {
                deviceDashboardDTOs.Add(_converter.convertDeviceToDashboardDTO(d));
            }

            return deviceDashboardDTOs;

        }
    }
}
