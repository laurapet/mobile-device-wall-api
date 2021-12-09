using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Lendings.Control
{
    public interface ILendingManagement
    {
        /// <summary>
        /// Gets the lendings assigned to a User from the LendingRepository
        /// and converts them to OwnLendingDTOs for presentation
        /// </summary>
        /// <param name="userId">The ID of the User the lending is assigned to</param>
        /// <returns>A List of OwnLendingDTOs</returns>
        public Task<IEnumerable<OwnLendingDTO>> GetOwnLendings(int userId);
        
        /// <summary>
        /// Modifies the Lending entity by replacing an old userId with a new one by using the LendingRepository
        /// </summary>
        /// <param name="lendingId">The ID of the lending to be modified</param>
        /// <param name="currentUserId">The ID of the current User assigned to the lending</param>
        /// <param name="newUserId">The ID of the new user to be assigned to the lending</param>
        /// <returns>No Content (204) if the operation was successful, 404 if the lendings or users can't be found</returns>
        public Task<ActionResult> ChangeUserOfLending(int lendingId, int currentUserId, int newUserId);
        
        /// <summary>
        /// Deletes a lending by using the LendingRepository
        /// </summary>
        /// <param name="lendingId">The ID of the lending to be deleted</param>
        /// <returns>No Content (204), 404 if the lending can't be found</returns>
        public Task<ActionResult> CancelLending(int lendingId);
        
        /// <summary>
        /// Get's a lending by its ID from the LendingRepository
        /// </summary>
        /// <param name="lendingId">The ID of the lending to be found</param>
        /// <returns>The lending with the given ID if it exists, 404 if it doesn't</returns>
        public Task<ActionResult<Lending>> GetLendingByID(int lendingId);
        public Task<ActionResult> LendDevices(List<LendingListDTO> lendingListDtos, int userId);
    }
}