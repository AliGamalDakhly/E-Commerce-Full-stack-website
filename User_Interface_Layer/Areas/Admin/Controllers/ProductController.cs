using Bussiness_Logic_Layer.IServices;
using Data_Access_Layer.Data.Context;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace User_Interface_Layer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        IProductService _productService;
        ICategoryService _categoryService;
        IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductService productService,
            ICategoryService categoryService,
            IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View(_productService.GetAll(null, p => p.Category));
        }

        //public IActionResult GetDataTable()
        //{
        //    var products = _productService.GetAll(null, p => p.Category);
        //    return Json(new { data = products });
        //}


        #region Create (Get and Post)
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_categoryService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile file)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Img");

            if (ModelState.IsValid)
            {
                if(file != null)
                {
                    string routPath = _webHostEnvironment.WebRootPath;
                    string randomFileName = Guid.NewGuid().ToString();
                    string fullPath = Path.Combine(routPath, @"Images/Products");
                    string fileExtension = Path.GetExtension(file.FileName);

                    using(var fileStream = new FileStream(Path.Combine(fullPath, $"{randomFileName}{fileExtension}"),FileMode.Create))
                    { file.CopyTo(fileStream); }

                    product.Img = $"/Images/Products/{randomFileName}{fileExtension}";
                }

                try
                {
                    _productService.AddProduct(product);
                    TempData["AdditionMsg"] = product.Name;
                }
                catch
                {
                    return View(product);
                }
                return RedirectToAction("Index");
            }

            return View();
        }
        #endregion

        #region Edit (Get and Post)

        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            ViewData["Categories"] = new SelectList(_categoryService.GetAll(), "Id", "Name");

            if (Id == null || Id == 0)
                NotFound();

            Product? product = _productService.GetProductById(Id);

            if (product == null)
                NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile file)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Img");
            ModelState.Remove("file");
            

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string routPath = _webHostEnvironment.WebRootPath;
                    string randomFileName = Guid.NewGuid().ToString();
                    string fullPath = Path.Combine(routPath, @"Images/Products");
                    string fileExtension = Path.GetExtension(file.FileName);

                    /* We have to delete old image from wwwroot */
                    if(product.Img != null)
                    {
                        // Remove the starting slash "/" if it exists
                        string oldRelativePath = product.Img.TrimStart('/');

                        // Combine wwwroot path with the relative path
                        string oldFullPath = Path.Combine(_webHostEnvironment.WebRootPath, oldRelativePath);

                        if (System.IO.File.Exists(oldFullPath))
                        {
                            System.IO.File.Delete(oldFullPath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(fullPath, $"{randomFileName}{fileExtension}"), FileMode.Create))
                    { file.CopyTo(fileStream); }

                    product.Img = $"/Images/Products/{randomFileName}{fileExtension}";
                }
                _productService.UpdateProduct(product);

                TempData["EditionMsg"] = product.Name;
                return RedirectToAction("Index");
            }

            ViewData["Categories"] = new SelectList(_categoryService.GetAll(), "Id", "Name");
            return View(product);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                NotFound();

            Product product = _productService.GetProductById(id);
            if (product != null)
            {
                _productService.DeleteProduct(id);
                TempData["DeletionMsg"] = product.Name;

                /* remove the image from wwwroot*/
                if (product.Img != null)
                {
                    // Remove the starting slash "/" if it exists
                    string oldRelativePath = product.Img.TrimStart('/');

                    // Combine wwwroot path with the relative path
                    string oldFullPath = Path.Combine(_webHostEnvironment.WebRootPath, oldRelativePath);

                    if (System.IO.File.Exists(oldFullPath))
                    {
                        System.IO.File.Delete(oldFullPath);
                    }
                }
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
