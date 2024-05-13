﻿namespace seaway.API.Models.ViewModels
{
    public class ActivityWithPicModel
    {
        public string? ActivityName { get; set; }
        public string? Description { get; set; }
        public bool ActivityIsActive { get; set; }
        public IFormFile[]? PicValue { get; set; }
    }
}