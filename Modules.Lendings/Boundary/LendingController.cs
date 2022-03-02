using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Modules.Lendings.Boundary
{
    /// <summary>
    /// Controller class for all API calls regarding lendings
    /// </summary>
    [ApiController]
    [Route("lendings")]
    public class LendingController : ControllerBase
    {
        private readonly ILendingManagement _lendingManagement;
        private readonly Microsoft.AspNetCore.Identity.UserManager<DeviceWallUser> _userManager;

        public LendingController(ILendingManagement lendingManagement, Microsoft.AspNetCore.Identity.UserManager<DeviceWallUser> userManager)
        {
            _lendingManagement = lendingManagement;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets all lendings of the current user
        /// </summary>
        /// <param name="userID">The ID of the current user</param>
        /// <returns>
        /// A List of all Lendings created by the current user if the User is authorized.
        /// Returns 401 if the User is not authorized.
        /// </returns>
        [Authorize]
        [HttpGet]
        public async Task <ActionResult<IEnumerable<OwnLendingDTO>>> GetOwnLendings()
        {
            return await _lendingManagement.GetOwnLendings(await getCurrentUser());
        }
        
        /// <summary>
        /// Gets a lending by its ID
        /// </summary>
        /// <param name="lendingID">The lendings ID</param>
        /// <returns>A lending with the given ID or 404 if there's no lending with the given ID</returns>
        [HttpGet("{lendingID}")]
        public async Task<ActionResult<Lending>> GetLendingByID(int lendingId)
        {
            return await _lendingManagement.GetLendingByID(lendingId);
        }

        /// <summary>
        /// Creates a lending of all devices and given in the provided list and assigns them to the user with the given userID
        /// </summary>
        /// <param name="lendingList">A List of DTOs containing a DeviceID and the IsLongterm attribute</param>
        /// <param name="userID">The userID of the user the lending is to be assigned to</param>
        /// <returns>201 if all Devices have been lent successfully. 400 if one of the Devices is already lent. 404 if one of the devices or the user doesn't exist</returns>
        [HttpPost]
        public async Task<ActionResult> LendDevices([FromBody]List<LendingListDTO> lendingList, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _lendingManagement.LendDevices(lendingList, user);
            }
            return NotFound();
        }

        /// <summary>
        /// Changes the user of a specific lending
        /// </summary>
        /// <param name="lendingID">The ID of the lending where the user is to be changed</param>
        /// <param name="newUserID">The ID of the new user that the lending is supposed to be assigned to</param>
        /// <returns>   A NoContentResult if the update was successful,
        ///             404 if no lending with the given ID or User with the given newUserId has been found.
        ///             401 if the user is not authorized.
        /// </returns>
        [Authorize]
        [HttpPut("{lendingId}")]
        public async Task<ActionResult> ChangeUserOfLending(int lendingId, int currentUserId ,string newUserId)
        {
            var newUser = await _userManager.FindByIdAsync(newUserId);
            return await _lendingManagement.ChangeUserOfLending(lendingId, currentUserId, newUser);
        }
        
        /// <summary>
        /// Cancels the lending with the given lendingID
        /// </summary>
        /// <param name="lendingID">The ID of the lending to be deleted</param>
        /// <returns>A NoContentResult if the deletion was successful, otherwise 404</returns>
        [HttpDelete("{lendingID}")]
        public async Task<ActionResult> CancelLending(int lendingID)
        {
            return await _lendingManagement.CancelLending(lendingID);
        }

        private async Task<DeviceWallUser> getCurrentUser()
        {
            return await _userManager.FindByIdAsync(User.Identity.GetUserId());
        }
        
        [HttpGet("lending-process")]
        public Task<ActionResult<Device>> GetDeviceForLendingProcess(int deviceId)
        {
            return _lendingManagement.GetDeviceForLendingProcess(deviceId);
        }
    }
}