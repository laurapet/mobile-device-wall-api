using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace device_wall_backend.Models
{
    public class DeviceWallUser: IdentityUser
    {
        public virtual ICollection<Lending> Lendings { get; set; }
    }
}