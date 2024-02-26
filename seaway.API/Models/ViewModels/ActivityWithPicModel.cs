namespace seaway.API.Models.ViewModels
{
    public class ActivityWithPicModel
    {
        public string? ActivityName { get; set; }
        public string? Description { get; set; }
        public bool? ActivityIsActive { get; set; }
        public string? PicType { get; set; }
        public string? PicName { get; set; }
        public List<string>? PicValue { get; set; }
    }
}
