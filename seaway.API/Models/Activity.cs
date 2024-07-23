namespace seaway.API.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string? ActivityName { get; set; }
        public string? Description { get; set; }
        public List<PicDocument>? ActivityPics { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? Updated { get; set; }
    }
}
