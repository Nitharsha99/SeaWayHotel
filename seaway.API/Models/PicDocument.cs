namespace seaway.API.Models
{
    public class PicDocument
    {
        public int? PicId {  get; set; }
        public int? PicTypeId { get; set; }
        public string? PicType { get; set;}
        public string? PicName { get; set;}
        public string? PicValue { get; set;}
        public bool? IsActive { get; set; }

    }
}
