namespace SportShop.Domain.Entities
{
    public class Category : EntityBase
    {
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
