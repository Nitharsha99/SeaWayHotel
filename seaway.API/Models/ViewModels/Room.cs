﻿namespace seaway.API.Models.ViewModels
{
    public class Room
    {
        public int Id { get; set; }
        public string? RoomNumber { get; set; }
        public int? RoomTypeId { get; set; }
        public string? RoomType { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? Created {  get; set; }
        public string? UpdatedBy { get;set; }
        public DateTime? Updated { get; set;}
    }
}
