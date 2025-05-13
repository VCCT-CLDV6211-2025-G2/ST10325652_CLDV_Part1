using System.ComponentModel.DataAnnotations;

namespace WebApplicationPOECLDV.Models
{
    public class Venue
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Venue name is required.")]
        public string? VenueName { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Capacity is required.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public string? ImageUrl { get; set; }

        public List<Booking> Bookings { get; set; } = new();
    }
}
