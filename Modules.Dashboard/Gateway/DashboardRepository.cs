using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using device_wall_backend.Modules.Dashboard.Control;
using device_wall_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Modules.Dashboard.Gateway
{
    public class DashboardRepository: IDashboardRepository
    {
        private readonly DashboardContext _dashboardContext;

        public DashboardRepository(DashboardContext dashboardContext)
        {
            _dashboardContext = dashboardContext;
        }

        //TODO: Filtern nach isTablet & isLent
        public async Task<IEnumerable<Device>> getDevicesForDashboard(DeviceFilter filter)
        {
            return await _dashboardContext.Devices.Where(d =>
            d.Features.OperatingSystem.Contains( filter.operatingSystem ?? String.Empty) &&
            d.Features.Version.Contains( filter.version ?? String.Empty )
                ).ToListAsync();
        }
    }
}
