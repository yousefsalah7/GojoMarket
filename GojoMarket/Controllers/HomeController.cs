using GojoMarket.Data;
using GojoMarket.Models;
using GojoMarket.Models.ViewModels;
using GojoMarket.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GojoMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                products = _db.Product.Include(u => u.Category),
                categories= _db.Category

            }; 
                
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<Cart> shoppingCart = new List<Cart>();
            if (HttpContext.Session.Get<List<Cart>>(WC.shoppingCart) != null && HttpContext.Session.Get<List<Cart>>(WC.shoppingCart).Count() > 0)
            {
                shoppingCart = HttpContext.Session.Get<List<Cart>>(WC.shoppingCart);
            }

            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(u => u.Category).Where(u => u.Id == id).First(),
                ExistInCart=false
            };
            foreach (var item in shoppingCart)
            {
                if (item.ProductId==id)
                {
                   detailsVM.ExistInCart = true;
                }
                //else
                //{
                //    detailsVM.ExistInCart = false;
                //}
            }

            return View(detailsVM);  
        }
        [HttpPost]
        public IActionResult Adding(int id)
        {
            List<Cart> shoppingCart = new List<Cart>();
            if (HttpContext.Session.Get<List<Cart>>(WC.shoppingCart)!=null && HttpContext.Session.Get<List<Cart>>(WC.shoppingCart).Count()>0) 
            {
                shoppingCart = HttpContext.Session.Get<List<Cart>>(WC.shoppingCart);
            }
            var product = _db.Product.Include(u => u.Category).Where(u => u.Id == id).First(); 
            shoppingCart.Add(new Cart { ProductId = id });
            HttpContext.Session.Set(WC.shoppingCart, shoppingCart);
            return RedirectToAction("Details",product);
        }
      
        public IActionResult Remove(int id)
        {
            List<Cart> shoppingCart = new List<Cart>();
            if (HttpContext.Session.Get<List<Cart>>(WC.shoppingCart) != null && HttpContext.Session.Get<List<Cart>>(WC.shoppingCart).Count() > 0)
            {
                shoppingCart = HttpContext.Session.Get<List<Cart>>(WC.shoppingCart);
            }
            var itemToRemove =shoppingCart.FirstOrDefault(u=>u.ProductId == id);
            if (itemToRemove != null)
            {
                shoppingCart.Remove(itemToRemove);
            }
            var product = _db.Product.Include(u => u.Category).Where(u => u.Id == id).First();
            HttpContext.Session.Set(WC.shoppingCart, shoppingCart);
            return RedirectToAction("Details", product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}