namespace seaway.API.Models
{
    public class Admin
    {
        public int? AdminId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? UserRole { get; set; }
        public DateTime? Created { get; set; }
    }
}
