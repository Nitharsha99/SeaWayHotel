namespace seaway.API.Models.ViewModels
{
    public class RoomCategoryWithPicModel
    {
        public string? RoomName { get; set; }
        public int? GuestCountMax { get; set; }
        public double? Price { get; set; }
        public double? DiscountPercentage { get; set; }
        public string? CreatedBy { get; set; }
        public RoomPic[]? roomPics { get; set; }
    }

    public class RoomPic
    {
        public string? PicName { get; set; }
        public string? PicValue { get; set; }
        public string? CloudinaryPublicId { get; set; }
    }
}
