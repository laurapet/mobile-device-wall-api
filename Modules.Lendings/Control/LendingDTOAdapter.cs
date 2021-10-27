using device_wall_backend.Models;
using device_wall_backend.Modules.Lendings.Control.DTOs;

namespace device_wall_backend.Modules.Lendings.Control
{
    public class LendingDTOAdapter
    {
        public OwnLendingDTO ConvertLendingToOwnDto(Lending lending)
        {
            OwnLendingDTO ownLendingDto = new OwnLendingDTO()
            {
                DeviceID = lending.DeviceID, 
                DeviceName = lending.Device.Name,
                OperatingSystem = lending.Device.OperatingSystem, 
                Version = lending.Device.Version,
                HorizontalSize = lending.Device.HorizontalSize, 
                VerticalSize = lending.Device.VerticalSize,
                IsTablet = lending.Device.IsTablet, 
                HasSIM = lending.Device.HasSIM,
                IsLongterm = lending.IsLongterm
            };

            return ownLendingDto;
        }
    }
}