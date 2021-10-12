public class Lending{
    public int LendingID{get;set;}
    public int UserID{get; set;}
    public int DeviceID{get; set;}
    public bool IsLongterm;

    public User User;
    public Device Device;
}