using Bussiness_Logic_Layer.IServices;
using Bussiness_Logic_Layer.Services;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace User_Interface_Layer.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private IProductService _productService;
        private IShoppingCartService _shoppingCartService;
        private IOrderItemService _orderItemService;
        private UserManager<AppUser> _userManager;
        
        public HomeController(IProductService productService,
            IShoppingCartService shoppingCartService,
            IOrderItemService orderItemService,
            UserManager<AppUser> userManager)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _orderItemService = orderItemService;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            int? quantity = 0;
            string? userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                ShoppingCart? cart = _shoppingCartService
                .GetAll(sc => sc.AppUserId == userId, sc => sc.OrderItems)
                .FirstOrDefault();

                if(cart != null)
                    quantity = cart.OrderItems?.Sum(o => o.Quantity);
            }

            ViewBag.CartTotalProductCount = quantity;
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

            int? quantity = 0;
            string? userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                ShoppingCart? cart = _shoppingCartService
                .GetAll(sc => sc.AppUserId == userId, sc => sc.OrderItems)
                .FirstOrDefault();

                if (cart != null)
                    quantity = cart.OrderItems?.Where(o => o.ProductId == Id)
                        .FirstOrDefault()?.Quantity ?? 0;
            }

            ViewBag.ProductCount = quantity;

            return View(product);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(int? Id, int quantity)
        {
            if (Id == null)
                NotFound();

            Product? product = _productService.GetProductById(Id);
            if (product == null)
                NotFound();

            string? currentUserId = _userManager.GetUserId(User);

            ShoppingCart? cart = _shoppingCartService
                .GetAll(sc => sc.AppUserId == currentUserId, sc => sc.OrderItems).FirstOrDefault();

            if (cart == null)
                NotFound();

            OrderItem? order = cart?.OrderItems.FirstOrDefault(o => o.ProductId == product.Id);
            if (order == null)
            {
                cart?.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    ShoppingCartId = cart.Id
                });
                _shoppingCartService.UpdateShoppingCart(cart);
            }
            else
            {
                order.Quantity = quantity;
                _shoppingCartService.UpdateShoppingCart(cart);
            }
            ViewBag.ProductCount = quantity;
            return View(product);
        }


        [Authorize]
        public IActionResult AddToCart(int? id)
        {
            if(id == null)
                NotFound();

            Product product = _productService.GetProductById(id);
            if(product == null)
                NotFound();

            string? currentUserId = _userManager.GetUserId(User);

            ShoppingCart? cart = _shoppingCartService
                .GetAll(sc => sc.AppUserId == currentUserId, sc => sc.OrderItems).FirstOrDefault();

            if(cart == null)
                NotFound();

            OrderItem? order =  cart?.OrderItems.FirstOrDefault(o => o.ProductId == product.Id);
            if(order == null)
            {
                cart?.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = 1,
                    ShoppingCartId = cart.Id
                });
                _shoppingCartService.UpdateShoppingCart(cart);
            }
            else
            {
                order.Quantity++;
                _shoppingCartService.UpdateShoppingCart(cart);
            }

            return RedirectToAction("Index");
        }
    }
}
