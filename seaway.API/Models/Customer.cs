namespace seaway.API.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email_add { get; set; }
        public string? ContactNo { get; set; }
        public string? PassportNo { get; set; }
        public string? NIC { get; set; }
        public DateTime? Created {  get; set; }
        public DateTime? Updated { get; set;}
    }
}
