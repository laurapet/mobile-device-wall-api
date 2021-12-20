using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Users.Control
{
    public interface ISearchManagement
    {
        public Task<ActionResult<IEnumerable<DeviceWallUser>>> GetUsersForSearch(string searchTerm);
    }
}