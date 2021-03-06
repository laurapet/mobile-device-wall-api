using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace device_wall_backend.Models{
    public class User
    {
        public int UserID{get;set;}
        public string Username{get;set;}
        public virtual ICollection<Lending> Lendings { get; set; }
    }
}