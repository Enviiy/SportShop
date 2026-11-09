namespace SportShop.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string? Size { get; set; }
        public string? Type { get; set; }
        public bool IsPromo { get; set; }
        public bool IsNew { get; set; }
    }
}
