using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using device_wall_backend.Modules.Dashboard.Gateway;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace device_wall_backend.Modules.Devices.Boundary
{
    [ApiController]
    [Route("devices")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceWallContext _context;

        public DeviceController(DeviceWallContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllDevices()
        {
            return Ok(await _context.Devices.ToListAsync()) ;
        }
        
        [HttpPost]
        public async Task<ActionResult<Device>> createDevice()
        {
            Device d1 = new() {Name = "iPhone 5", OperatingSystem = "iOS", Version = "10.3.4"};
            Device d2 = new() {Name = "Samsung Galaxy S5",OperatingSystem = "Android", Version = "6.0.1"};
            _context.Devices.Add(d1);
            _context.Devices.Add(d2);

            await _context.SaveChangesAsync();
            return Created("", d1);
        }

        /*[HttpPut("{deviceID}")]
        public async Task<ActionResult> updateDevice(int deviceID)
        {
            var device = await _context.Devices.FindAsync(deviceID);
            
        }*/
    }
}