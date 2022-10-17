namespace GojoMarket.Models.ViewModels
{
    public class UserProducts
    {
        public UserProducts()
        {
            products = new List<Product>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Product> products   { get; set; }
    }
}
