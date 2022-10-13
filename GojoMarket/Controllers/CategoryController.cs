using GojoMarket.Data;
using GojoMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace GojoMarket.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _db.Category;
            return View(categories);
        }
        //get - Create 
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category item)
        {
            if (ModelState.IsValid) { 
            _db.Category.Add(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edite(int? id)
        {
            if (id == null || id ==0) {
                return NotFound();
            }
            var item = _db.Category.Find(id);
            if (item == null) 
            
            { 
                return NotFound(); 
            } 
            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category item)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(item);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(Category item)
        {
            
            _db.Category.Remove(item);
            _db.SaveChanges();
           
            return RedirectToAction("Index");
        }
    }
}
