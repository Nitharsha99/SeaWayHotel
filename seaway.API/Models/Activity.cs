namespace seaway.API.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string? ActivityName { get; set; }
        public string? Description { get; set; }
        public List<PicDocument>? ActivityPics { get; set; }
        public bool IsActive { get; set; }
    }
}
