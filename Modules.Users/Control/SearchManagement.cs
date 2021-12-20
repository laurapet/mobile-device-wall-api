using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Users.Gateway;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Users.Control
{
    public class SearchManagement: ISearchManagement
    {
        private readonly IUserRepository _userRepository;

        public SearchManagement(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ActionResult<IEnumerable<DeviceWallUser>>> GetUsersForSearch(string searchTerm)
        {
            return await _userRepository.GetUsersForSearch(searchTerm);
        }
    }
}