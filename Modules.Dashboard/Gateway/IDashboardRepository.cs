using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Dashboard.Control;
using device_wall_backend.Modules.Dashboard.Control.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Dashboard.Gateway
{
    public interface IDashboardRepository
    {
        public Task<IEnumerable<Device>> GetDevicesForDashboard(DeviceFilter filter);

        public Task<ActionResult<Device>> GetDeviceDetails(int deviceId);
    }
}