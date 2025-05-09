namespace WebApplicationPOECLDV.Models
{
    public class BookingDisplayViewModel
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }

        public string EventName { get; set; }
        public DateTime EventDate { get; set; }

        public string VenueName { get; set; }
        public int Capacity { get; set; }

    }
}

