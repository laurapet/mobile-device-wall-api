using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Modules.Dashboard.Control.DTOs;

namespace device_wall_backend.Modules.Dashboard.Control
{
    public interface IDashboardManagement
    {
        public Task<IEnumerable<DeviceDashboardDTO>> getDevicesForDashboard(DeviceFilter filter);

    }
}
