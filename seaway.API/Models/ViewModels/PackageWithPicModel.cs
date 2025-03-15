using seaway.API.Models.Enum;

namespace seaway.API.Models.ViewModels
{
    public class PackageWithPicModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public PackageDurationType DurationType { get; set; }
        public double Price { get; set; }
        public UserType UserType { get; set; }
        public PackagePic[]? packagePics { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

    }

    public class PackagePic
    {
        public string? PicName { get; set; }
        public string? PicValue { get; set; }
        public string? CloudinaryPublicId { get; set; }
    }
}
