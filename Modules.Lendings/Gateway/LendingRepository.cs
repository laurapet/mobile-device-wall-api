using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;
using Microsoft.AspNetCore.Mvc;

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
                User = new User() {Username = "u3"}, UserID = 3, IsLongterm = lendingDto.IsLongterm
            };

            _context.Lendings.Add(lendingToCreate);
            _context.SaveChangesAsync();
            return lendingToCreate;
        }
    }
}