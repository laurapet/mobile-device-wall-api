using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace device_wall_backend.Modules.Lendings.Gateway
{
    public class LendingRepository: ILendingRepository
    {
        private readonly DeviceWallContext _context;

        public LendingRepository(DeviceWallContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lending>> GetOwnLendings(int userId)
        {
            var lendings = await _context.Lendings.Include(lending => lending.Device).Where(lending => lending.DeviceWallUser.Id == userId).ToListAsync();
            return lendings;
        }

        public async Task<ActionResult> UpdateUserOfLending(int lendingId, int currentUserId, DeviceWallUser newUser)
        {
            var lending = await _context.Lendings.FindAsync(lendingId);
            _context.Entry(lending).Reference(d => d.DeviceWallUser).Load();
            if (lending == null)
            {
                return new NotFoundObjectResult(new {message = "lending " + lendingId + " not found."});
            }
            
            if (newUser == null)
            {
                return new NotFoundObjectResult(new {message = "user " + newUser.Id + " not found."});
            }
            
            if (lending.DeviceWallUser.Id != currentUserId)
            {
                return new ForbidResult();
            }
            
            lending.DeviceWallUser = newUser;
            _context.Entry(lending).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return new NoContentResult();
        }

        public async Task<ActionResult> DeleteLending(int lendingId)
        {
            var lending = await _context.Lendings.FindAsync(lendingId);
            if (lending == null)
            {
                return new NotFoundObjectResult(new {message = "lending "+lendingId+" not found."});
            }

            _context.Lendings.Remove(lending);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<ActionResult<Lending>> GetLendingByID(int lendingId)
        {
            var lending = await _context.Lendings.FindAsync(lendingId);
            if (lending == null)
            {
                return new NotFoundObjectResult(new {message = "lending "+lendingId+" not found."});
            }
            return lending;
        }

        public async Task<ActionResult> CreateLendings(List<LendingListDTO> lendingListDtos, DeviceWallUser user)
        {
            List<Lending> lendingsToCreate = new List<Lending>();
            foreach (var lendingListDto in lendingListDtos)
            {
                var deviceToLend = await _context.Devices.FindAsync(lendingListDto.DeviceID);
                if (deviceToLend == null)
                {
                    return new NotFoundObjectResult(new {message = "device "+lendingListDto.DeviceID+" not found."});
                }
            
                _context.Entry(deviceToLend).Reference(d => d.CurrentLending).Load();
                if (deviceToLend.CurrentLending != null)
                {
                    return new BadRequestObjectResult(new {message = "device "+lendingListDto.DeviceID+" is already lent."});
                }

                var lending = new Lending()
                {
                    Device = deviceToLend, 
                    DeviceID = deviceToLend.DeviceID,
                    DeviceWallUser = user,
                    IsLongterm = lendingListDto.IsLongterm
                };
                lendingsToCreate.Add(lending);
            }
            
            _context.Lendings.AddRange(lendingsToCreate);
            await _context.SaveChangesAsync();
            return new StatusCodeResult(201);
        }

        public async Task<ActionResult<Device>> GetDeviceForLendingProcess(int deviceId)
        {
            var device = await _context.Devices.FindAsync(deviceId);

            if (device == null)
            {
                return new NotFoundResult();
            }
            _context.Entry(device).Reference(d => d.CurrentLending).Load();
            if (device.CurrentLending != null)
            {
                return new BadRequestResult();
            }
            return device;
            
        }
    }
}