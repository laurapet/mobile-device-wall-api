using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using device_wall_backend.Modules.Lendings.Gateway;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Lendings.Control
{
    public class LendingManagement: ILendingManagement
    {
        private readonly ILendingRepository _lendingRepository;

        public LendingManagement(ILendingRepository lendingRepository)
        {
            _lendingRepository = lendingRepository;
        }
        
        //TODO: Convert DTO to Lending
        public async Task<ActionResult<Lending>> LendDevice(LendingDTO lendingDTO, int userID)
        {
            return await _lendingRepository.CreateLending(lendingDTO, userID);
        }
    }
}