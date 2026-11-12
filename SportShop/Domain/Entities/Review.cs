namespace SportShop.Domain.Entities
{
    public class Review : EntityBase
    {
        public string? UserId { get; set; }          // IdentityUser.Id
        public string? AuthorName { get; set; }
        public string? Text { get; set; }
        public int Rating { get; set; }              // 1..5
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public bool IsApproved { get; set; }         // модерация админом
    }
}
