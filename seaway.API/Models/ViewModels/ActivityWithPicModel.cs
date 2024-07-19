namespace seaway.API.Models.ViewModels
{
    public class ActivityWithPicModel
    {
        public string? ActivityName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy {  get; set; }
        public string? UpdatedBy { get; set; }
        public ActivityPic[]? ActivityPics { get; set; }
    }

    public class ActivityPic
    {
        public string? PicName { get; set; }
        public string? PicValue { get; set; }
        public string? CloudinaryPublicId { get; set; }
    }
}
