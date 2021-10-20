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
            //Eager loading: When the entity is read, related data is retrieved along with it. This typically results in a single join query that retrieves all of the data that's needed.
            //You specify eager loading in Entity Framework Core by using the Include and ThenInclude methods.
            var lendingDevices = _context.Devices.Include(d => d.currentLending).ThenInclude(l=>l.User).ToList();
            
            IQueryable<Device> deviceFilterResult = _context.Devices.Where(d =>
                    d.OperatingSystem.Contains(filter.operatingSystem ?? string.Empty) &&
                    d.Version.Contains(filter.version ?? string.Empty));

            if (filter.isTablet != null)
            {
                deviceFilterResult = deviceFilterResult.Where(d => d.IsTablet == filter.isTablet);
            }

            if (filter.isLent != null)
            {
                if (filter.isLent==true)
                {
                    deviceFilterResult = deviceFilterResult.Where(d => d.currentLending != null);
                }
                else
                {
                    deviceFilterResult = deviceFilterResult.Where(d => d.currentLending == null);
                }
            }
            return await deviceFilterResult.ToListAsync();
            //return await deviceFilterQuery.ToListAsync();
        }
    }
}