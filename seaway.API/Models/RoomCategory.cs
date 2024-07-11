namespace seaway.API.Models
{
    public class RoomCategory
    {
        public int? RoomCategoryId { get; set; }
        public string? RoomName { get; set; }
        public int? GuestCountMax { get; set; }
        public double? Price { get; set; }
        public double? DiscountPercentage { get; set; }
        public double? DiscountAmount { get; set; }
        public string? CreatedBy {  get; set; }
        public DateTime? Created { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public List<PicDocument>? RoomPics { get; set; }
    }
}
