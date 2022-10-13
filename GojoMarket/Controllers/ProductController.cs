using GojoMarket.Data;
using GojoMarket.Models;
using GojoMarket.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GojoMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
      
        public ProductController(ApplicationDbContext db,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
           _webHostEnvironment=webHostEnvironment;

        }

        public IActionResult Index()
        {
            IEnumerable<Product> ProductList = _db.Product;
            foreach (var item in ProductList)
            {
                item.Category = _db.Category.FirstOrDefault(u => u.Id == item.CategoryId);
            }
            return View(ProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(
                i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };
            //IEnumerable<SelectListItem> selectListItems = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name.ToString(),
            //    Value = i.Id.ToString(),
            //    Selected = true
            //}) ;
            //ViewBag.CategoryList=selectListItems;
            //Product product = new Product();
            if (id.HasValue)
            {
               productVM.Product = _db.Product.Find(id);
                return View(productVM);
            }
            return View(productVM);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            
           
                var webrootbath = _webHostEnvironment.WebRootPath;
                var Uploadedfiles = HttpContext.Request.Form.Files;

                if (productVM.Product.Id==0)
                {
                    //creating 
                    string upload = webrootbath + WC.imagePath;
                    string fileName=Guid.NewGuid().ToString();  
                    string extention = Path.GetExtension(Uploadedfiles[0].FileName) ;

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                    {
                        Uploadedfiles[0].CopyTo(fileStream);
                    }
                    productVM.Product.Image = fileName+extention;
                    _db.Product.Add(productVM.Product);
                    
                }
                else
                {
                    //updating
                    var oldObj = _db.Product.AsNoTracking().FirstOrDefault(u=>u.Id==productVM.Product.Id);
                    if (Uploadedfiles.Count>0)
                    {
                        string pathh=webrootbath + WC.imagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(Uploadedfiles[0].FileName);
                        using ( var Streamer = new FileStream(Path.Combine(pathh,fileName+extension),FileMode.Create) )
                        {
                            Uploadedfiles[0].CopyTo(Streamer);
                        }

                        
                        var oldFile = Path.Combine(pathh, oldObj.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        productVM.Product.Image = fileName + extension;

                        }
                        else
                        {
                            productVM.Product.Image = oldObj.Image;
                        }

                    _db.Product.Update(productVM.Product);
                }
                   _db.SaveChanges();

                return RedirectToAction("Index");
               
            
        }

        public IActionResult Delete(Product product)
{
            var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(i => i.Id == product.Id);
            string root = _webHostEnvironment.WebRootPath;
            var upload = root + WC.imagePath;
            var oldfile = Path.Combine(upload,objFromDb.Image);
            System.IO.File.Delete(oldfile);
            _db.Product.Remove(product);
                     _db.SaveChanges();
                     return RedirectToAction("Index");
        }
    }
}


