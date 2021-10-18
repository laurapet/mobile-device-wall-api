using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Dashboard.Control;
using device_wall_backend.Modules.Dashboard.Control.DTOs;
using device_wall_backend.Modules.Lendings.Gateway;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace device_wall_backend.Modules.Dashboard.Gateway
{
    public class DashboardRepository: IDashboardRepository
    {
        private readonly DeviceWallContext _context;
        private readonly ILogger _logger;

        public DashboardRepository(DeviceWallContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Device>> getDevicesForDashboard(DeviceFilter filter)
        {
            IQueryable<Device> deviceFilterQuery = _context.Devices.Where(d =>
                    d.OperatingSystem.Contains(filter.operatingSystem ?? string.Empty) &&
                    d.Version.Contains(filter.version ?? string.Empty));

            if (filter.isTablet != null)
            {
                deviceFilterQuery = deviceFilterQuery.Where(d => d.IsTablet == filter.isTablet);
            }
            return await deviceFilterQuery.ToListAsync();
        }
    }
}