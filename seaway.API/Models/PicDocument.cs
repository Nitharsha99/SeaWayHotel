namespace seaway.API.Models
{
    public class PicDocument
    {
        public int? PicTypeId {  get; set; }
        public string? PicType { get; set;}
        public string? PicName { get; set;}
        public string? PicValue { get; set;}
        public string? CloudinaryPublicId { get; set;}
        public string? CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? Updated { get; set; }

    }
}
