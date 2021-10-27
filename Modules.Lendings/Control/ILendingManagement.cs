using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Lendings.Control
{
    public interface ILendingManagement
    {
        public Task<ActionResult<Lending>> LendDevice(LendingDTO lendingDTO, int userId);
        public Task<IEnumerable<OwnLendingDTO>> GetOwnLendings(int userId);
        public Task<ActionResult> ChangeUserOfLending(int lendingId, int currentUserId, int newUserId);
        public Task<ActionResult> CancelLending(int lendingId, int userId);
    }
}