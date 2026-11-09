namespace SportShop.Domain.Entities
{
    public class Brand : EntityBase
    {
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
