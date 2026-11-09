using SportShop.Domain.Entities;

namespace SportShop.Models;

public class CartViewModel
{
    public IEnumerable<CartItem> Items { get; set; } = new List<CartItem>();
    public decimal Total => Items.Sum(i => i.Quantity * (i.Product?.Price ?? 0));
    public int Count => Items.Sum(i => i.Quantity);
}