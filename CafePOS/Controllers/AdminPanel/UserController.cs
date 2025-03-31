using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafePOS.Controllers.AdminPanel
{
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class UserController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(AppDbContext context, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
            //var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User does not exit");
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var model = new Users
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                CurrentRoles = (List<string>)userRoles,
                AvailableRoles = allRoles.Select(role => role.Name!).ToList(),
            };

            return View(model);
        }

        [Route("user/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(Users user)
        {
            var existedUser = await _userManager.FindByIdAsync(user.Id);
            if (existedUser is null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(existedUser);
            await _userManager.RemoveFromRolesAsync(existedUser, currentRoles);           
            await _userManager.AddToRolesAsync(existedUser, new List<string> { user.NewRole });

            existedUser.Id = user.Id;
            existedUser.Email = user.Email;
            existedUser.UserName = user.UserName;
            existedUser.FullName = user.FullName;            
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
