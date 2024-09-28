namespace seaway.API.Models.ViewModels
{
    public class BookingAdd
    {
        public DateTime BookingDate { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestCount { get; set; }
        public int RoomCount { get; set; }
        public string Name { get; set; }
        public string Email_add { get; set; }
        public string ContactNo { get; set; }
        public string? PassportNo { get; set; }
        public string? NIC { get; set; }
    }
}
