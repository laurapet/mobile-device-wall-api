using System.Threading.Tasks;
using device_wall_backend.Gateway;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using device_wall_backend.Models;
using device_wall_backend.Modules.Dashboard.Control;
using device_wall_backend.Modules.Dashboard.Gateway;

namespace device_wall_backend.Modules.Dashboard.Boundary
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardContext _context;
        private readonly IDashboardManagement _dashboardManagement;

        public DashboardController(DashboardContext context, IDashboardManagement dashboardManagement)
        {
            _context = context;
            _dashboardManagement = dashboardManagement;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllDevices([FromQuery]DeviceFilter filter)
        {
            return Ok(await _dashboardManagement.getDevicesForDashboard(filter)) ;
        }

        /*[HttpPost]
        public async Task<ActionResult<Lending>> lendDevice()
        {
            Lending l = new() { UserID = 1, DeviceID = 1, IsLongterm = true, Device = new Device { Name = "d", DeviceID = 1 }, User = new User { Username = "u", UserID = 1 } };

            if (_context!=null)
            {
                _context.Lendings.Add(l);
            }
            
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return Created("",l);
        }*/
    }
}
