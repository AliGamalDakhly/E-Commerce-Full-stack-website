using Bussiness_Logic_Layer.IServices;
using Data_Access_Layer.Data.Context;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace User_Interface_Layer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public IActionResult Index()
        {
            return View(_categoryService.GetAll());
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
                _categoryService.AddCategory(category);
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

            Category? category = _categoryService.GetCategoryById(Id);

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
                _categoryService.UpdateCategory(category);
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

            _categoryService.DeleteCategory(id);

            return RedirectToAction("Index");
        }
        #endregion
    }
}
