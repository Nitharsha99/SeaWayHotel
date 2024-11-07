using seaway.API.Models.Enum;

namespace seaway.API.Models
{
    public class Package
    {
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public PackageDurationType DurationType { get; set; }
        public double Price { get; set; }
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public List<PicDocument>? PackagePics { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set;}
    }
}
