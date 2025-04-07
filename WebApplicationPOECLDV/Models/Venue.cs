namespace WebApplicationPOECLDV.Models
{
    public class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string ImageURL { get; set; }
        public List<Booking> Bookings { get; set; } = new();
    }
}
