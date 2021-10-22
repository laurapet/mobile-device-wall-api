using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Dashboard.Control.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Dashboard.Control
{
    public interface IDashboardManagement
    {
        public Task<IEnumerable<DeviceDashboardDTO>> GetDevicesForDashboard(DeviceFilter filter);

        public Task<ActionResult<Device>> GetDeviceDetails(int deviceID);
    }
}
