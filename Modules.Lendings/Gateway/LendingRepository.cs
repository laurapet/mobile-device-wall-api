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

        public async Task<ActionResult<Lending>> CreateLending(LendingDTO lendingDto, int UserID)
        {
            Device deviceToLend = await _context.Devices.FindAsync(lendingDto.DeviceID);
            if (deviceToLend == null)
            {
                return new NotFoundResult();
            }
            _context.Entry(deviceToLend).Reference(d => d.CurrentLending).Load();
            if (deviceToLend.CurrentLending != null)
            {
                return new BadRequestResult();
            }
            
            //TODO: User finden
            Lending lendingToCreate = new Lending()
            {
                Device = deviceToLend, DeviceID = deviceToLend.DeviceID,
                User = new User() {Username = "u3"}, UserID = 3, IsLongterm = lendingDto.IsLongterm// wird noch durch was nicht hardgecodedetes ersetzt
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
            //TODO: Include UserTable? gucken, ob user berechtigter user ist & ob es newUser überhaupt gibt?
            var lending = await _context.Lendings.FindAsync(lendingId);
            if (lending == null)
            {
                return new NotFoundResult();
            }

            lending.UserID = newUserId;
            _context.Entry(lending).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return new NoContentResult();
        }
    }
}