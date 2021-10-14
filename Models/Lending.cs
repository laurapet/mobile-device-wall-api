namespace device_wall_backend.Models{
    public class Lending{
        public int LendingID{get;set;}
        public int UserID{get; set;}
        public int DeviceID{get; set;}
        public bool IsLongterm;

        public User User;
        public Device Device;
    }
}