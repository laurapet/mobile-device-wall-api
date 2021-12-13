using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control;
using device_wall_backend.Modules.Lendings.Control.DTOs;
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

        public LendingController(ILendingManagement lendingManagement)
        {
            _lendingManagement = lendingManagement;
        }

        /// <summary>
        /// Gets all lendings of the current user
        /// </summary>
        /// <param name="userID">The ID of the current user</param>
        /// <returns>A List of all Lendings created by the current user.</returns>
        [HttpGet]
        public async Task <ActionResult<IEnumerable<OwnLendingDTO>>> GetOwnLendings()
        {
            if (User.Identity.IsAuthenticated)
            {
                return await _lendingManagement.GetOwnLendings(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            return new UnauthorizedResult();
        }
        
        /// <summary>
        /// Gets a lending by its ID
        /// </summary>
        /// <param name="lendingID">The lendings ID</param>
        /// <returns>A lending with the given ID or 404 if there's no lending with the given ID</returns>
        [HttpGet("{lendingID}")]
        public async Task<ActionResult<Lending>> GetLendingByID(int lendingID)
        {
            return await _lendingManagement.GetLendingByID(lendingID);
        }

        /// <summary>
        /// Creates a lending of all devices and given in the provided list and assigns them to the user with the given userID
        /// </summary>
        /// <param name="lendingList">A List of DTOs containing a DeviceID and the IsLongterm attribute</param>
        /// <param name="userID">The userID of the user the lending is to be assigned to</param>
        /// <returns>201 if all Devices have been lent successfully. 400 if one of the Devices is already lent. 404 if one of the devices doesn't exist</returns>
        [HttpPost]
        public async Task<ActionResult> LendDevices([FromBody]List<LendingListDTO> lendingList, int userID)
        {
            return await _lendingManagement.LendDevices(lendingList, userID);
        }

        /// <summary>
        /// Changes the user of a specific lending
        /// </summary>
        /// <param name="lendingID">The ID of the lending where the user is to be changed</param>
        /// <param name="currentUserID">The ID of the current user that is assigned to the lending</param>
        /// <param name="newUserID">The ID of the new user that the lending is supposed to be assigned to</param>
        /// <returns>A NoContentResult if the update was successful, otherwise 404</returns>
        /// TODO: current User aus Header/ Securitycontext nehmen
        [HttpPut("{lendingID}")]
        public async Task<ActionResult> ChangeUserOfLending(int lendingID, int newUserID)
        {
            if (User.Identity.IsAuthenticated)
            {
                return await _lendingManagement.ChangeUserOfLending(lendingID,Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), newUserID);
            }
            return new UnauthorizedResult();
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
    }
}