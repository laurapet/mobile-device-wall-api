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
    /// <summary>
    /// Controller class for all API calls regarding lendings
    /// </summary>
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

        /// <summary>
        /// Gets all lendings
        /// </summary>
        /// <returns>A List of all lendings</returns>
        //TODO: Admin-Beschränkung?
        [HttpGet]
        public async Task<ActionResult> GetAllLendings()
        {
            return Ok(await _context.Lendings.ToListAsync()) ;
        }

        /// <summary>
        /// Gets all lendings of the current user
        /// </summary>
        /// <param name="userID">The ID of the current user</param>
        /// <returns>A List of all Lendings created by the current user</returns>
        //TODO: wenn OAuth eingebunden ist, userID aus securitykontext holen? Fehler falls keine ID im Kontext gesetzt ist
        [HttpGet("{userID}")]
        public async Task<ActionResult<IEnumerable<OwnLendingDTO>>> GetOwnLendings(int userID)
        {
            return Ok(await _lendingManagement.GetOwnLendings(userID));
        }

        /// <summary>
        /// Creates a lending of all devices and given in the provided list and assigns them to the user with the given userID
        /// </summary>
        /// <param name="lendingList">A List of DTOs containing a DeviceID and the IsLongterm attribute</param>
        /// <param name="userID">The userID of the user the lending is to be assigned to</param>
        /// <returns>A List of the results of each created lending. A result can contain the succesfully created Lending or a Statuscode indicating the creation didn't succeed</returns>
        //TODO: ist das problematisch, dass man pro Gerät, das man ausleihen will entweder das erstellte Lending zurückbekommt oder einen Fehlercode? 
        //TODO: soll ggf. ein insgesamter Fehlercode zurückgegeben werden falls ein Gerät nicht ausgeliehen werden konnte?
        [HttpPost]
        public async Task<IEnumerable<ActionResult<Lending>>> LendDevice([FromBody]List<LendingListDTO> lendingList, int userID)
        {
            List <ActionResult<Lending>> lendingResults = new List<ActionResult<Lending>>();
            foreach (var lendingListDto in lendingList)
            {
                LendingDTO l = new LendingDTO(){DeviceID = lendingListDto.DeviceID, UserID = userID, IsLongterm = lendingListDto.IsLongterm};
                lendingResults.Add(await _lendingManagement.LendDevice(l, userID));
            }
            return lendingResults;
        }

        /// <summary>
        /// Changes the user of a specific lending
        /// </summary>
        /// <param name="lendingID">The ID of the lending where the user is to be changed</param>
        /// <param name="currentUserID">The ID of the current user that is assigned to the lending</param>
        /// <param name="newUserID">The ID of the new user that the lending is supposed to be assigned to</param>
        /// <returns>A NoContentResult if the update was successful, otherwise 404</returns>
        [HttpPut("{lendingID}")]
        public async Task<ActionResult> ChangeUserOfLending(int lendingID, int currentUserID, int newUserID)
        {
            return await _lendingManagement.ChangeUserOfLending(lendingID, currentUserID, newUserID);
        }
        
        /// <summary>
        /// Cancels the lending with the given lendingID
        /// </summary>
        /// <param name="lendingID">The ID of the lending to be deleted</param>
        /// <returns>A NoContentResult if the deletion was successful, otherwise 404</returns>
        //TODO: Canceln funktioniert über einscannen mit hardware, bei zentralem Tablet sollte man zur Rückgabe nicht wieder einloggen müssen. Deswegen keine Übergabe der userID?
        [HttpDelete("{lendingID}")]
        public async Task<ActionResult> CancelLending(int lendingID)
        {
            return await _lendingManagement.CancelLending(lendingID);
        }
    }
}