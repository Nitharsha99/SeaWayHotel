namespace seaway.API.Models
{
    public class PicDocument
    {
        public int? PicTypeId {  get; set; }
        public string? PicType { get; set;}
        public string? PicName { get; set;}
        public IFormFile? PicValue { get; set;}
        public byte[]? PicValueInByte { get; set;}

    }
}
