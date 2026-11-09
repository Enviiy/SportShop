using SportShop.Domain.Enums;

namespace SportShop.Domain.Entities
{
    public class Order : EntityBase
    {
        public string? UserId { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public decimal Total { get; set; }
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.New;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
