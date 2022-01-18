using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Lendings.Gateway
{
    public interface ILendingRepository
    {
        /// <summary>
        /// Gets all Lendings from the Database that are assigned to the given userID
        /// </summary>
        /// <param name="userId">The ID of the User</param>
        /// <returns>A List of Lendings</returns>
        public Task<IEnumerable<Lending>>GetOwnLendings(int userId);
        
        /// <summary>
        /// Modifies the Lending entity by replacing an old userId with a new one
        /// </summary>
        /// <param name="lendingId">The ID of the lending to be modified</param>
        /// <param name="currentUserId">The ID of the current User assigned to the lending</param>
        /// <param name="newUserId">The ID of the new user to be assigned to the lending</param>
        /// <returns>No Content (204) if the operation was succesful, 404 if the lendings or users can't be found</returns>
        public Task<ActionResult> UpdateUserOfLending(int lendingId, int currentUserId, DeviceWallUser newUser);
        
        /// <summary>
        /// Deletes a lending entity
        /// </summary>
        /// <param name="LendingID">the ID of the lending to be deleted</param>
        /// <returns>No Content (204), 404 if the lending can't be found</returns>
        public Task<ActionResult> DeleteLending(int lendingID);
        
        /// <summary>
        /// Get's a lending by its ID from the database
        /// </summary>
        /// <param name="lendingId">The ID of the lending to be found</param>
        /// <returns>The lending with the given ID if it exists, 404 if it doesn't</returns>
        public Task<ActionResult<Lending>> GetLendingByID(int lendingId);
        
        /// <summary>
        /// Creates Lendings by converting lendingListDtos to lending entities and assigning them to a user with the given ID 
        /// </summary>
        /// <param name="lendingListDtos">A List of DTOs containing lending information</param>
        /// <param name="userId">The ID of the user the lending is to be assigned to</param>
        /// <returns>201 if the creation was successful, 400 if one of the given devices is already lent, 404 if the user or the device with a given ID can't be found</returns>
        public Task<ActionResult> CreateLendings(List<LendingListDTO> lendingListDtos, DeviceWallUser user);

        public Task<ActionResult<Device>> GetDeviceForLendingProcess(int deviceId);
    }
}