namespace TicketSystem.Models
{
    public class EventInformation
    {

        public EventInformation(bool registered,int sold,int available, int checkedIn)
        {
            Registered = registered;
            Sold = sold;
            Available = available;
            CheckedIn = checkedIn;
        }
        public bool Registered { get; set; }
        public int Sold { get; set; }
        public int Available { get; set; }
        public int CheckedIn { get; set; }
    }
}   