namespace seaway.API.Models
{
    public class Room
    {
        public int? RoomId { get; set; }
        public string? RoomName { get; set; }
        public int? GuestCountMax { get; set; }
        public double? Price { get; set; }
        public double? DiscountPercentage { get; set; }
        public double? DiscountAmount { get; set; }
        public List<PicDocument>? RoomPics { get; set; }
    }
}
