namespace seaway.API.Models.ViewModels
{
    public class AdminWithProfilePic
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public IFormFile? ProfilePic { get; set; }
    }
}
