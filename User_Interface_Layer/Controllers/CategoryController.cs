using Data_Access_Layer.Data.Context;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace User_Interface_Layer.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext dbContext;
        public CategoryController(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }


        public IActionResult Index()
        {
            return View(dbContext.Categories.ToList());
        }


        #region Create (Get and Post)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            ModelState.Remove("Products");

            if (ModelState.IsValid)
            {
                dbContext.Categories.Add(category);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
        #endregion

        #region Edit (Get and Post)

        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if(Id == null || Id == 0)
                NotFound();

            Category? category = dbContext.Categories.Find(Id);

            if (category == null)
                NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            ModelState.Remove("Products");

            if (ModelState.IsValid)
            {
                dbContext.Categories.Update(category);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
                NotFound();

            Category? category = dbContext.Categories.Find(id);

            if (category == null)
                NotFound();

            dbContext.Categories.Remove(category);
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
