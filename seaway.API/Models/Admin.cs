﻿namespace seaway.API.Models
{
    public class Admin
    {
        public int? AdminId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
        public string? ProfilePicPath { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? Created {  get; set; }
        public string? UpdatedBy {  get; set; }
        public DateTime? Updated { get; set; }
    }
}
