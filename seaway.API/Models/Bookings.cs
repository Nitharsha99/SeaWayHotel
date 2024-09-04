namespace seaway.API.Models
{
    public class Bookings
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
