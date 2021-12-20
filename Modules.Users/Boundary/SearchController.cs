using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Users.Control;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Users.Boundary
{
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchManagement _searchManagement;

        public SearchController(ISearchManagement searchManagement)
        {
            _searchManagement = searchManagement;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceWallUser>>> SearchUsers(string searchTerm)
        {
            return await _searchManagement.GetUsersForSearch(searchTerm);
        }
    }
}