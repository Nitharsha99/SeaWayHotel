namespace seaway.API.Models
{
    public class Room
    {
        public string? RoomName { get; set; }
        public int? GuestCountMax { get; set; }
        public double? Price { get; set; }
        public double? DiscountPercentage { get; set; }
        public bool? IsActive { get; set; }
    }
}
