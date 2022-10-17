using GojoMarket.Data;
using GojoMarket.Models;
using GojoMarket.Models.ViewModels;
using GojoMarket.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace GojoMarket.Controllers
{
   
    public class CartController : Controller
    {
        [BindProperty]  
        public UserProductVM UserProductVM { get; set; }
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
    
        public IActionResult Index()
        {
            List<Cart> ItemsInCart = new List<Cart>();

            if (HttpContext.Session.Get<List<Cart>>(WC.shoppingCart) != null
                  && HttpContext.Session.Get<List<Cart>>(WC.shoppingCart).Count>0 )
            {
                // there is items in the cart 
                ItemsInCart = HttpContext.Session.Get<List<Cart>>(WC.shoppingCart);
            };

            List<int> ProdsInCart = ItemsInCart.Select(i => i.ProductId).ToList();
            IEnumerable<Product> selectedProducts = _db.Product.Where(u => ProdsInCart.Contains(u.Id));          
             return View(selectedProducts);  
        }
        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPpst()
        {
            return RedirectToAction(nameof(Summry));
        }
            [Authorize]
     
        public IActionResult Summry()
        {
            List<Cart> ItemsInCart = new List<Cart>();
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            if (HttpContext.Session.Get<List<Cart>>(WC.shoppingCart) != null
                  && HttpContext.Session.Get<List<Cart>>(WC.shoppingCart).Count>0 )
            {
                // there is items in the cart 
                ItemsInCart = HttpContext.Session.Get<List<Cart>>(WC.shoppingCart);
            };

            List<int> ProdsInCart = ItemsInCart.Select(i => i.ProductId).ToList();
            IEnumerable<Product> selectedProducts = _db.Product.Where(u => ProdsInCart.Contains(u.Id));

            UserProductVM = new UserProductVM()
            {
                applicationUser = _db.applicationUsers.FirstOrDefault(u => u.Id == claim.Value),
                products=selectedProducts,
            };
             return View(UserProductVM);  
        }


        [HttpPost]
        [ActionName("Summry")]
        public IActionResult SummryPost(UserProductVM UserProductVM)
        {
            
            return RedirectToAction(nameof(Inquiry));
        }
        public IActionResult Inquiry()
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult Delete(int id)
        {
            List<Cart> ItemsInCart = new List<Cart>();
            ItemsInCart = HttpContext.Session.Get<List<Cart>>(WC.shoppingCart);
            ItemsInCart.Remove(ItemsInCart.FirstOrDefault(u=>u.ProductId==id));

            HttpContext.Session.Set<List<Cart>>(WC.shoppingCart, ItemsInCart);

            //List<int> ProdsInCart = ItemsInCart.Select(i => i.ProductId).ToList();
            //IEnumerable<Product> selectedProducts = _db.Product.Where(u => ProdsInCart.Contains(u.Id));
            return RedirectToAction("Index");
        }
    }
}
