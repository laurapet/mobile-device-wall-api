﻿namespace device_wall_backend.Modules.Dashboard.Control.DTOs
{
    public class CurrentLendingDTO
    {
        public int LendingID { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsLongterm { get; set; }
    }
}