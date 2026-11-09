using SportShop.Domain.Repositories.Abstract;

namespace SportShop.Domain.Repositories
{
    public class DataManager
    {
        public IProductsRepository Products { get; set; }
        public ICategoriesRepository Categories { get; set; }
        public IBrandsRepository Brands { get; set; }
        public IReviewsRepository Reviews { get; set; }
        public IOrdersRepository Orders { get; set; }
        public ICartRepository Cart { get; set; }

        public DataManager(
            IProductsRepository products,
            ICategoriesRepository categories,
            IBrandsRepository brands,
            IReviewsRepository reviews,
            IOrdersRepository orders,
            ICartRepository cart)
        {
            Products = products;
            Categories = categories;
            Brands = brands;
            Reviews = reviews;
            Orders = orders;
            Cart = cart;
        }
    }
}