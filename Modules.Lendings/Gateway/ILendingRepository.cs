using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Lendings.Gateway
{
    public interface ILendingRepository
    {
        public Task<IEnumerable<Lending>>GetOwnLendings(int userId);
        public Task<ActionResult> UpdateUserOfLending(int lendingId, int currentUserId, int newUserId);
        public Task<ActionResult> DeleteLending(int lendingID);
        public Task<ActionResult<Lending>> GetLendingByID(int lendingId);
        public Task<ActionResult> CreateLendings(List<LendingListDTO> lendingListDtos, int userId);
    }
}