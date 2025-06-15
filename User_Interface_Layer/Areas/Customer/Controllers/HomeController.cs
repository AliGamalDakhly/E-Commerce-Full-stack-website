using Bussiness_Logic_Layer.IServices;
using Bussiness_Logic_Layer.Services;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace User_Interface_Layer.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private IProductService _productService;
        
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }


        public IActionResult Index()
        {
            return View(_productService.GetAll());
        }

        [HttpGet]
        public IActionResult Details(int? Id)
        {
            if(Id == null) 
                NotFound();

            Product? product = _productService.GetProductById(Id);

            if (product == null)
                NotFound();

            return View(product);
        }
    }
}
