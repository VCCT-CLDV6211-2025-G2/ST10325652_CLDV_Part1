namespace WebApplicationPOECLDV.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public List<Booking> Bookings { get; set; } = new();
    }
}
