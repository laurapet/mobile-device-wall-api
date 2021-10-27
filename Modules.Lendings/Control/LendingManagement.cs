using System.Collections;
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
        private readonly LendingDTOAdapter _converter;

        public LendingManagement(ILendingRepository lendingRepository)
        {
            _lendingRepository = lendingRepository;
            _converter = new LendingDTOAdapter();
        }
        
        //TODO: Convert DTO to Lending
        public async Task<ActionResult<Lending>> LendDevice(LendingDTO lendingDTO, int userId)
        {
            return await _lendingRepository.CreateLending(lendingDTO, userId);
        }

        public async Task<IEnumerable<OwnLendingDTO>> GetOwnLendings(int userId)
        {
            List<OwnLendingDTO> ownLendingDTOs = new List<OwnLendingDTO>();
            var ownLendings = await _lendingRepository.GetOwnLendings(userId);
            
            foreach (Lending lending in ownLendings)
            {
                ownLendingDTOs.Add(_converter.convertLendingToOwnDTO(lending));
            }

            return ownLendingDTOs;
        }

        public async Task<ActionResult> ChangeUserOfLending(int lendingId, int currentUserId, int newUserId)
        {
            return await _lendingRepository.UpdateUserOfLending(lendingId, currentUserId, newUserId);
        }

        public async Task<ActionResult> CancelLending(int lendingId, int userId)
        {
            return await _lendingRepository.DeleteLending(lendingId, userId);
        }
    }
}