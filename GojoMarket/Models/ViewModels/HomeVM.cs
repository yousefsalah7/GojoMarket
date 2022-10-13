namespace GojoMarket.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Category> categories { get; set; }

    }
}
