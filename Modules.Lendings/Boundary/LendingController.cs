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

        //TODO: Admin-Beschränkung?
        [HttpGet]
        public async Task<ActionResult> GetAllLendings()
        {
            return Ok(await _context.Lendings.ToListAsync()) ;
        }

        //TODO: wenn OAuth eingebunden ist, userID aus securitykontext holen?
        [HttpGet("{userID}")]
        public async Task<ActionResult<IEnumerable<OwnLendingDTO>>> GetOwnLendings(int userID)
        {
            //falls im securitycontext keine userID/Token gesetzt ist 403
            return Ok(await _lendingManagement.GetOwnDevices(userID));
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

        [HttpPut("{lendingID}")]
        public async Task<ActionResult> ChangeUserOfLending(int lendingID, int currentUserID, int newUserID)
        {
            return await _lendingManagement.ChangeUserOfLending(lendingID, currentUserID, newUserID);
        }
    }
}