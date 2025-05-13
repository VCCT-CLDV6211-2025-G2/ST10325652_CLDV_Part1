using System.ComponentModel.DataAnnotations;

namespace WebApplicationPOECLDV.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Venue is required.")]
        public int VenueId { get; set; }

        public Venue? Venue { get; set; }

        public int EventId { get; set; }

        [Required(ErrorMessage = "Event is required.")]
        public Event? Event { get; set; }

        [Required(ErrorMessage = "Booking Date is required.")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

    }
}
