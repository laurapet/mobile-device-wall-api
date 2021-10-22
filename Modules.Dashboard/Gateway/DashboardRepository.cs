using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using device_wall_backend.Modules.Dashboard.Control;
using Microsoft.AspNetCore.Mvc;
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
        
        public async Task<IEnumerable<Device>> GetDevicesForDashboard(DeviceFilter filter)
        {
            //Eager loading: When the entity is read, related data is retrieved along with it. This typically results in a single join query that retrieves all of the data that's needed.
            //You specify eager loading in Entity Framework Core by using the Include and ThenInclude methods.
            var lendingDevices = _context.Devices.Include(device => device.CurrentLending).ThenInclude(lending => lending.User);
            var deviceFilterResult = lendingDevices.Where(device =>
                    device.OperatingSystem.Contains(filter.OperatingSystem ?? string.Empty) &&
                    device.Version.Contains(filter.Version ?? string.Empty));

            if (filter.IsTablet != null)
            {
                deviceFilterResult = deviceFilterResult.Where(device => device.IsTablet == filter.IsTablet);
            }

            if (filter.IsLent != null)
            {
                bool filterIsLent = filter.IsLent.Value; //to make bool not nullable, so ternary expression works
                deviceFilterResult = deviceFilterResult.Where(device => filterIsLent ? device.CurrentLending != null : device.CurrentLending == null);
            }
            
            return await deviceFilterResult.ToListAsync(); 
        }

        public async Task<ActionResult<Device>> GetDeviceDetails(int deviceId)
        {
            var device = await _context.Devices.FindAsync(deviceId);
            if (device == null)
            {
                return new NotFoundResult();
            }
            return device;
        }
    }
}