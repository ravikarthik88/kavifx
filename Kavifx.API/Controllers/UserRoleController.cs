using Kavifx.API.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kavifx.API.Controllers
{
    [EnableCors("CrossPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserRoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAndRoles()
        {
            UserRoleViewModel viewModel = new UserRoleViewModel();
            List<SelectListItem> Users = new List<SelectListItem>();
            List<SelectListItem> Roles = new List<SelectListItem>();
            var appUsers = await _userManager.Users.Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();
            foreach (var user in appUsers)
            {
                Users.Add(new SelectListItem
                {
                    Text = user.Email,
                    Value = user.Email
                });
            }

            var appRoles = await _roleManager.Roles.Where(x => x.IsDeleted == false).ToListAsync();
            foreach(var role in appRoles)
            {
                Roles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name
                });
            }

            viewModel.Users = Users;
            viewModel.Roles = Roles;

            return Ok(viewModel);
        }

        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var users = await _userManager.Users.Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();
            var userWithRoles = new List<UserInRoleViewModel>();            
            foreach(var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var roles = string.Join(",", userRoles);
                userWithRoles.Add(new UserInRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles
                });
            }

            return Ok(userWithRoles);
        }

        [HttpGet("GetUserRoles/{id}")]
        public async Task<IActionResult> GetUserRoles(int id)
        {
            var user = await _userManager.Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = new UserInRoleViewModel();
            foreach (var role in userRoles) 
            {
                userRole.UserId = id;
                userRole.Email = user.Email;
                userRole.Roles = role;                
            }
            return Ok(userRole);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserwithRoles(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id == id && x.IsActive == true && x.IsDeleted ==false);
            UpdateUserInRoleViewModel userWithRoles = new UpdateUserInRoleViewModel();
            List<SelectListItem> models = new List<SelectListItem>();
            var userRoles = await _roleManager.Roles.Where(x =>x.IsDeleted == false).ToListAsync();
            userWithRoles.Id = user.Id;
            userWithRoles.Email = user.Email;

            foreach(var role in userRoles)
            {
                models.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name
                });
            }

            userWithRoles.Roles = models;
            return Ok(userWithRoles);
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole([FromBody]AssignUserToRoleViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("user is not found");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return NotFound("role does not exist");
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok("The User is Added to Role Successfully");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserRole(int id,[FromBody]AssignUserToRoleViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("user not found");
            }

            var rolesToremove = await _userManager.GetRolesAsync(user);
            foreach (var roleName in rolesToremove)
            {
                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    var removeFromRoleResult = await _userManager.RemoveFromRoleAsync(user, roleName);
                    if (!removeFromRoleResult.Succeeded)
                    {
                        return BadRequest("error removing user from current role");
                    }
                }
            }
            
            var addToRoleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (!addToRoleResult.Succeeded)
            {
                return BadRequest("error adding user to new role");
            }
            return Ok("User role updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUserFromRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("user not found");
            }
            var rolesToremove = await _userManager.GetRolesAsync(user);
            foreach (var roleName in rolesToremove)
            {
                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    var removeFromRoleResult = await _userManager.RemoveFromRoleAsync(user, roleName);
                    if (!removeFromRoleResult.Succeeded)
                    {
                        return BadRequest("error removing user from current role");
                    }
                }
            }
            return Ok("User removed from role successfully");
        }        
    }
}
