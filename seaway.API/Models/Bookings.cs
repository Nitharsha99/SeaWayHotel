namespace seaway.API.Models
{
    public class Bookings
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestCount { get; set; }
        public int RoomCount { get; set; }
        public DateTime Created {  get; set; }
        public DateTime Updated { get; set; }
    }
}
