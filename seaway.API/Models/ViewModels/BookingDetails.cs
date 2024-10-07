namespace seaway.API.Models.ViewModels
{
    public class BookingDetails
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestCount { get; set; }
        public int RoomCount { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Name { get; set; }
        public string Email_add { get; set; }
        public string ContactNo { get; set; }
        public string? PassportNo { get; set; }
        public string? NIC { get; set; }
        public List<BookingRoom>? bookingRooms { get; set; }
    }

    public class BookingRoom
    {
        public int BookingRoomId { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
    }
}
