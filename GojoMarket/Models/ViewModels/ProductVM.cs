using Microsoft.AspNetCore.Mvc.Rendering;

namespace GojoMarket.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; } 
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }    

    }
}
