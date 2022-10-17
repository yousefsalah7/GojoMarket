namespace GojoMarket.Models.ViewModels
{
    public class UserProductVM
    {
        public UserProductVM()
        {
            products = new List<Product>();
        }
        public ApplicationUser applicationUser{ get; set; }
        public IEnumerable<Product> products { get; set; }
    }
}
