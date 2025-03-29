using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafePOS.Controllers.AdminPanel
{
    [Route("admin/")]
    public class UserController : Controller
    {
        private readonly UserManager<Users> _userManager;

        public UserController(AppDbContext context, UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        [Route("users")]
        public async Task<IActionResult> Users()
        {
            var Users = _userManager.Users.OrderByDescending(user => user.FullName);
            return View(Users);
        }

        [Route("user/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
            return View(user);
        }

        [Route("user/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, Users user)
        {
            var existedUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (existedUser is null) return NotFound();
            existedUser.Id = id;
            existedUser.Email = user.Email;           
            existedUser.UserName = user.UserName;
            existedUser.FullName = user.FullName;
            existedUser.UserRole = user.UserRole;
            existedUser.UserStatus = user.UserStatus;

            await _userManager.UpdateAsync(existedUser);
            return RedirectToAction("Users");
        }


        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user is null) return NotFound();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Users");
        }

    }
}
