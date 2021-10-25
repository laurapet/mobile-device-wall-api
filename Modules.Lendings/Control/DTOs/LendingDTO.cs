namespace device_wall_backend.Modules.Lendings.Control.DTOs
{
    public class LendingDTO
    {
        public int DeviceID { get; set; }
        public int UserID { get; set; }
        public bool IsLongterm { get; set; }
    }
}