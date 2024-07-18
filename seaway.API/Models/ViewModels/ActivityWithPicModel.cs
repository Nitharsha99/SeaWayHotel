namespace seaway.API.Models.ViewModels
{
    public class ActivityWithPicModel
    {
        public string? ActivityName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? Created {  get; set; }
        public string? Updated { get; set; }
        public ActivityPic[]? ActivityPics { get; set; }
    }

    public class ActivityPic
    {
        public string? PicName { get; set; }
        public string? PicValue { get; set; }
        public string? CloudinaryPublicId { get; set; }
    }
}
