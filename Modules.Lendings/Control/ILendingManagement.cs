using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Lendings.Control
{
    public interface ILendingManagement
    {
        public Task<ActionResult<Lending>> LendDevice(LendingDTO lendingDTO, int UserID);
        public Task<IEnumerable<OwnLendingDTO>> GetOwnDevices(int userId);
        public Task<ActionResult> ChangeUserOfLending(int LendingID, int CurrentUserID, int NewUserID);
    }
}