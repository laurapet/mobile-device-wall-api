using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using device_wall_backend.Models;
using device_wall_backend.Modules.Dashboard.Control;

namespace device_wall_backend.Modules.Dashboard.Boundary
{
    /// <summary>
    /// class for all API-calls regarding the Dashboard of the Device Wall to get an overview of all Devices and their properties
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardManagement _dashboardManagement;

        public DashboardController(IDashboardManagement dashboardManagement)
        {
            _dashboardManagement = dashboardManagement;
        }

        /// <summary>
        /// Gets all available devices based on the values of the DeviceFilter
        /// </summary>
        /// <param name="filter">The filter's properties define which properties the returned devices should have</param>
        /// <returns>A list representing devices with desired properties</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllDevices([FromQuery]DeviceFilter filter)
        {
            return Ok(await _dashboardManagement.GetDevicesForDashboard(filter)) ;
        }
        
        /// <summary>
        /// Gets a Device by its ID
        /// </summary>
        /// <param name="deviceID">The device's ID</param>
        /// <returns>A device if one with the provided deviceID exists, otherwise 404</returns>
        [HttpGet("{deviceID}")]
        public async Task<ActionResult<Device>> GetDeviceDetails(int deviceID)
        {
            return await _dashboardManagement.GetDeviceDetails(deviceID);
        }
    }
}
