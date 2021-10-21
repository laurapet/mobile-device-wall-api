using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Modules.Lendings.Boundary
{
    [ApiController]
    [Route("lendings")]
    public class LendingController : ControllerBase
    {
        private readonly DeviceWallContext _context;

        public LendingController(DeviceWallContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLendings()
        {
            return Ok(await _context.Lendings.ToListAsync()) ;
        }

        [HttpPost]
        public async Task<ActionResult<Lending>> LendDevice()
        {
            Lending l = new() { UserID = 1, DeviceID = 1, IsLongterm = true, Device = new Device { Name = "d"}, User = new User { Username = "u"} };

            if (_context!=null)
            {
                _context.Lendings.Add(l);
            }
            
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return Created("",l);
        }
    }
}