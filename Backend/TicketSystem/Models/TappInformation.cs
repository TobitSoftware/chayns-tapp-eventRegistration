namespace TicketSystem.Models
{
    public class TappInformation
    {
        public TappInformation() { }
        public TappInformation(string secret, int registerUacGroupId, int checkInUacGroupId, int ticketCount)
        {
            Secret = secret;
            RegisterUacGroupId = registerUacGroupId;
            CheckInUacGroupId = checkInUacGroupId;
            TicketCount = ticketCount;
        }

        public string Secret { get; set; }
        public int RegisterUacGroupId { get; set; }
        public int CheckInUacGroupId { get; set; }
        public int TicketCount { get; set; }
    }
}