namespace WebApplicationPOECLDV.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<Booking> Bookings { get; set; } = new();
    }
}
