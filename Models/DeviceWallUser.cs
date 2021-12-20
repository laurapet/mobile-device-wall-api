using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace device_wall_backend.Models
{
    public class DeviceWallUser: IdentityUser<int>
    {
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Lending> Lendings { get; set; }
        
    }
}