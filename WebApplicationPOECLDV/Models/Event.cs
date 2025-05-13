using System.ComponentModel.DataAnnotations;

namespace WebApplicationPOECLDV.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Event date is required.")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public string ImageUrl { get; set; }

        public List<Booking> Bookings { get; set; } = new();
    }
}
