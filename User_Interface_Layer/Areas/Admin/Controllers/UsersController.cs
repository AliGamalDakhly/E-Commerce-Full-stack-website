using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using User_Interface_Layer.ViewModels;

namespace User_Interface_Layer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;


        public UsersController(RoleManager<IdentityRole> roleManager,
                UserManager<AppUser> userManager,
                IUserStore<AppUser> userStore)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore(); 
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
                return RedirectToPage("/Identity/Account/Login.cshtml");

            return View(_userManager.Users.Where(u => u.Id != user.Id).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUserViewModel? appUser)
        {
            ViewData["Roles"] = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");


            if (ModelState.IsValid)
            {
                var user = new AppUser();

                await _userStore.SetUserNameAsync(user, appUser.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, appUser.Email, CancellationToken.None);
                user.Name = appUser.Name;
                user.Address = appUser.Address;
                var result = await _userManager.CreateAsync(user, appUser.Password);

                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, appUser.Role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(appUser);
        }


        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }
    }
}
