using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Modules.Dashboard.Control;
using device_wall_backend.Models;

namespace device_wall_backend.Modules.Dashboard.Gateway
{
    public interface IDashboardRepository
    {
        public Task<IEnumerable<Device>> getDevicesForDashboard(DeviceFilter filter);
    }
}
