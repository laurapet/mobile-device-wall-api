using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
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

        //TODO: ausprobieren
        public async Task<ActionResult<Lending>> CreateLending(LendingDTO lendingDto, int userId)
        {
            var deviceToLend = await _context.Devices.FindAsync(lendingDto.DeviceID);
            if (deviceToLend == null)
            {
                return new NotFoundResult();
            }
            
            _context.Entry(deviceToLend).Reference(d => d.CurrentLending).Load();
            if (deviceToLend.CurrentLending != null)
            {
                return new BadRequestResult();
            }

            var user = await _context.Users.FindAsync(userId);
            var lendingToCreate = new Lending()
            {
                Device = deviceToLend, DeviceID = deviceToLend.DeviceID,
                User = user
            };

            _context.Lendings.Add(lendingToCreate);
            await _context.SaveChangesAsync();
            return lendingToCreate;
        }

        public async Task<IEnumerable<Lending>> GetOwnLendings(int userId)
        {
            return await _context.Lendings.Include(lending => lending.Device).Where(lending => lending.UserID == userId).ToListAsync();
        }

        public async Task<ActionResult> UpdateUserOfLending(int lendingId, int currentUserId, int newUserId)
        {
            var lending = await _context.Lendings.FindAsync(lendingId);
            var newUser = await _context.Users.FindAsync(newUserId);
            if (lending == null || newUser == null)
            {
                return new NotFoundResult();
            }
            
            if (lending.UserID != currentUserId)//Or not Admin?
            {
                return new StatusCodeResult(403);// 'new ForbidResult()' requires Authenticationscheme
            }
            
            lending.UserID = newUserId;
            _context.Entry(lending).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return new NoContentResult();
        }

        public async Task<ActionResult> DeleteLending(int lendingID, int currentUserID)
        {
            var lending = await _context.Lendings.FindAsync(lendingID);
            if (lending == null)
            {
                return new NotFoundResult();
            }

            _context.Lendings.Remove(lending);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}