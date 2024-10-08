namespace seaway.API.Models.ViewModels
{
    public class AdminWithProfilePic
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
        public string? PicPath { get; set; }
        public string? PicName { get; set; }
        public string? PublicId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
