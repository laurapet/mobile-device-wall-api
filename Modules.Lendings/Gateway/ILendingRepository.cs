using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Lendings.Gateway
{
    public interface ILendingRepository
    {
        public Task<ActionResult<Lending>> CreateLending(LendingDTO lendingDto, int userID);
    }
}