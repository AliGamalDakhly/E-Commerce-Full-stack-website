using System.Linq;
using System.Threading.Tasks;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using User_Interface_Layer.ViewModels;

namespace User_Interface_Layer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
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


        public async Task<IActionResult> LockUnLock(string? Id)
        {
            if (Id == null)
                NotFound();

            AppUser? appUser = await _userManager.FindByIdAsync(Id);

            if (appUser == null)
                NotFound();

            bool isLocked  = await _userManager.IsLockedOutAsync(appUser);

            if(isLocked)
                await _userManager.SetLockoutEndDateAsync(appUser, DateTime.UtcNow);
            else
                await _userManager.SetLockoutEndDateAsync(appUser, DateTime.UtcNow.AddDays(2));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string? Id)
        {
            if(Id == null)
                NotFound();

            AppUser? appUser = await _userManager.FindByIdAsync(Id);

            if (appUser == null)
                NotFound();


            var result =  await _userManager.DeleteAsync(appUser);

            return RedirectToAction("Index");
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
