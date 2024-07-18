namespace seaway.API.Models.ViewModels
{
    public class RoomCategoryWithPicModel
    {
        public string? RoomName { get; set; }
        public int? GuestCountMax { get; set; }
        public double? Price { get; set; }
        public double? DiscountPercentage { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public RoomPic[]? roomPics { get; set; }
        public string? Created { get; set; }
        public string? Updated { get; set; }
    }

    public class RoomPic
    {
        public string? PicName { get; set; }
        public string? PicValue { get; set; }
        public string? CloudinaryPublicId { get; set; }
    }
}
