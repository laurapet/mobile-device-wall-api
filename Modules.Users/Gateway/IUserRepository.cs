using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Users.Gateway
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets Users whose usernames and full names contain the given search term.
        /// </summary>
        /// <param name="searchTerm">A String that must be contained in all returning usernames or names of the returning Users</param>
        /// <returns>A list of DeviceWallUser objects</returns>
        public Task<ActionResult<IEnumerable<DeviceWallUser>>> GetUsersForSearch(string searchTerm);
    }
}