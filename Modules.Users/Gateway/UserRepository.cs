using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Modules.Users.Gateway
{
    public class UserRepository: IUserRepository
    {
        private readonly DeviceWallContext _context;


        public UserRepository(DeviceWallContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<DeviceWallUser>>> GetUsersForSearch(string searchTerm)
        {
            return await _context.DeviceWallUsers.Where(user => user.UserName.Contains(searchTerm)).ToListAsync();
        }
    }
}