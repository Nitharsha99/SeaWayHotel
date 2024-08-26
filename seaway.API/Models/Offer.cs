namespace seaway.API.Models
{
    public class Offer
    {
        public int OfferId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set;}
        public double? Price { get; set; }
        public double? DiscountPercentage { get; set; }
        public double? DiscountAmount { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set;}
        public bool IsActive { get; set; }
        public bool? IsRoomOffer { get; set;}
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime Updated { get; set; }
        public List<PicDocument>? OfferPics { get; set; }
    }
}
