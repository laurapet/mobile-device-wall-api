using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Modules.Lendings.Boundary
{
    [ApiController]
    [Route("lendings")]
    public class LendingController : ControllerBase
    {
        private readonly DeviceWallContext _context;
        private readonly ILendingManagement _lendingManagement;

        public LendingController(DeviceWallContext context, ILendingManagement lendingManagement)
        {
            _context = context;
            _lendingManagement = lendingManagement;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLendings()
        {
            return Ok(await _context.Lendings.ToListAsync()) ;
        }

        [HttpPost]
        public async Task<IEnumerable<ActionResult<Lending>>> LendDevice([FromBody]List<LendingListDTO> lendingList, int userID)
        {
            List <ActionResult<Lending>> lendingResults = new List<ActionResult<Lending>>();
            foreach (var lendingDTO in lendingList)
            {
                LendingDTO l = new LendingDTO(){DeviceID = lendingDTO.DeviceID, UserID = userID, IsLongterm = lendingDTO.IsLongterm};
                lendingResults.Add(await _lendingManagement.LendDevice(l, userID));
            }

            return lendingResults;
        }
    }
}