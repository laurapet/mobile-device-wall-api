using System.ComponentModel.DataAnnotations;

namespace device_wall_backend.Models{
    public class Lending{
        [Key]
        public int DeviceID{get; set;}
        //public int UserID{get; set;}
        public bool IsLongterm{get; set;}

        //public User User{get; set;}
        public virtual DeviceWallUser DeviceWallUser { get; set; }
        public Device Device{get; set;}
    }
}