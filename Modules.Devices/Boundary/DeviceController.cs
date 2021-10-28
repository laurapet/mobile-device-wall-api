using System.Linq;
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
        
        [HttpGet("{deviceID}")]
        public async Task<ActionResult<Device>> GetDeviceByID(int deviceID)
        {
            var device = await _context.Devices.FindAsync(deviceID);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }
        
        [HttpPost]
        public async Task<ActionResult<Device>> CreateDevice(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeviceByID), new{ id = device.DeviceID }, device);
        }

        [HttpPut("{deviceID}")]
        public async Task<ActionResult> UpdateDevice(int deviceID, Device device)
        {
            var d = await _context.Devices.FindAsync(deviceID);
            if (d == null)
            {
                return NotFound();
            }
            _context.Entry(device).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        //TODO: entweder warning in form von bad request zurücksenden, falls device in lending steckt,
        //TODO: sonst einfach deleten und ggf. confirm funktion machen die lending von device mit löscht
        [HttpDelete("{deviceID}")]
        public async Task<IActionResult> DeleteTodoItem(long deviceID)
        {
            var deviceToDelete = await _context.Devices.FindAsync(deviceID);
            if (deviceToDelete == null)
            {
                return NotFound();
            }
            //TODO: gucken, ob das includen für ein cascade delete reicht; ref: https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete
            var deviceLendings = _context.Devices.Include(device => device.CurrentLending)
                .Where(device => device.DeviceID == deviceToDelete.DeviceID);
            
            _context.Devices.Remove(deviceToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}