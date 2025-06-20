using System.Threading.Tasks;
using Bussiness_Logic_Layer.IServices;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace User_Interface_Layer.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IShoppingCartService _shoppingCartService;
        private IOrderItemService _orderItemService;

        public CartController(UserManager<AppUser> userManager,
            IShoppingCartService shoppingCartService,
            IOrderItemService orderItemService) 
        {
            _shoppingCartService = shoppingCartService;
            _userManager = userManager;
            _orderItemService = orderItemService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            ShoppingCart? cart = _shoppingCartService
                    .GetAll(sc => sc.AppUserId == userId)
                    .FirstOrDefault();

            List<OrderItem> orderItms = _orderItemService
                .GetAll(o => o.ShoppingCartId == cart.Id, o => o.Product)
                .ToList();

            decimal totalPrice = orderItms.Sum(o => (o.Quantity * o.Product.Price));

            ViewData["totalPrice"] = totalPrice;

            return View(orderItms);
        }


        public IActionResult increment(int id)
        {

            OrderItem? orderItm = _orderItemService
                .GetAll(o => o.Id == id, o => o.Product)
                .FirstOrDefault();

            orderItm.Quantity++;

            _orderItemService.UpdateOrderItem(orderItm);

            return RedirectToAction("Index");
        }

        public IActionResult decrement(int id)
        {

            OrderItem? orderItm = _orderItemService
                .GetAll(o => o.Id == id, o => o.Product)
                .FirstOrDefault();
            if(orderItm?.Quantity > 0)
            {
                orderItm.Quantity--;
            }
            

            if(orderItm.Quantity == 0)
                _orderItemService.DeleteOrderItem(id);
            else
                _orderItemService.UpdateOrderItem(orderItm);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            AppUser? user = await _userManager.GetUserAsync(User);
            ViewData["CurrentUser"] = user;

            ShoppingCart? cart = _shoppingCartService
                    .GetAll(sc => sc.AppUserId == user.Id)
                    .FirstOrDefault();

            List<OrderItem> orderItms = _orderItemService
                .GetAll(o => o.ShoppingCartId == cart.Id, o => o.Product)
                .ToList();

            decimal totalPrice = orderItms.Sum(o => (o.Quantity * o.Product.Price));

            ViewData["totalPrice"] = totalPrice;

            return View(orderItms);
        }
    }
}
