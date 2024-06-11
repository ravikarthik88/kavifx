using Kavifx.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kavifx.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext ctx;

        public RolePermissionController(RoleManager<AppRole> roleManager, AppDbContext context)
        {
            _roleManager = roleManager;
            ctx = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesandPermissions()
        {
            RolePermissionViewModel rolePermission = new RolePermissionViewModel();
            List<SelectListItem> Permissions = new List<SelectListItem>();
            List<SelectListItem> Roles = new List<SelectListItem>();

            var permissions = await ctx.Permissions.Where(x => x.IsDeleted == false).ToListAsync();
            foreach(var permit in permissions)
            {
                Permissions.Add(new SelectListItem
                {
                    Text = permit.Name,
                    Value = permit.Name
                });
            }

            var appRoles = await _roleManager.Roles.Where(x => x.IsDeleted == false).ToListAsync();
            foreach (var role in appRoles)
            {
                Roles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name
                });
            }

            rolePermission.Permissions = Permissions;
            rolePermission.Roles = Roles;
            return Ok(rolePermission);
        }

        [HttpGet("GetRolePermissions")]
        public async Task<IActionResult> GetRolePermissions()
        {
            var roles = await _roleManager.Roles.Where(x => x.IsDeleted == false).ToListAsync();
            var roleswithpermissions = new List<PermissionInRoleViewModel>();
            foreach (var role in roles)
            {
                var rolePermissions = await ctx.RolePermissions.Where(x => x.Role.Id == role.Id).Select(x => x.Permission.Name).ToListAsync();
                var permissions = string.Join(",", rolePermissions);
                roleswithpermissions.Add(new PermissionInRoleViewModel
                {
                    Id = role.Id,
                    RoleName = role.Name,
                    Permissions = permissions
                });
            }

            return Ok(roleswithpermissions);
        }

        [HttpGet("GetRolePermissions/{id}")]
        public async Task<IActionResult> GetRolePermissions(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);            
            var rolePermissions = await ctx.RolePermissions.Where(x => x.Role.Id == role.Id).Select(x => x.Permission.Name).ToListAsync();
            var permissions = string.Join(",", rolePermissions);
            var rolewithpermissions = new PermissionInRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Permissions = permissions
            };
            return Ok(rolewithpermissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleswithPermissions(string id)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            UpdatePermissionInRoleViewModel updatePermissionInRole = new UpdatePermissionInRoleViewModel();
            List<SelectListItem> models = new List<SelectListItem>();
            var permissions = await ctx.Permissions.Where(x => x.IsDeleted == false).ToListAsync();
            updatePermissionInRole.RoleName = role.Name;
            foreach(var permit in permissions)
            {
                models.Add(new SelectListItem
                {
                    Text = permit.Name,
                    Value = permit.Name
                });
            }
            updatePermissionInRole.Permissions = models;
            return Ok(updatePermissionInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignRolePermissionViewModel model)
        {
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null) 
            {
                return NotFound("Role Not Found");
            }

            var permission = await ctx.Permissions.Where(x => x.Name == model.PermissionName).FirstOrDefaultAsync();
            if (permission == null)
            {
                return NotFound("Permission Not Found");
            }

            var result = await _roleManager.AddClaimAsync(role, new Claim("permission", permission.Name));
            if (result.Succeeded)
            {
                return Ok("Permission assigned to role");
            }
            else
            {
                return BadRequest("Failed to assign permission to role");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermissionToRole(string id, [FromBody] AssignRolePermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound("user not found");
            }

            var permissionToRemove = await _roleManager.GetClaimsAsync(role);
            
            foreach (var roleName in permissionToRemove)
            {
                var removeFromRoleResult = await _roleManager.RemoveClaimAsync(role, roleName);
                if (!removeFromRoleResult.Succeeded)
                {
                    return BadRequest("error removing permission from current role");
                }
            }

            var permission = await ctx.Permissions.Where(x => x.Name == model.PermissionName).Select(x=>x.Id).FirstOrDefaultAsync();
            var permissionName = new Claim("permission", model.PermissionName);
            var addToRoleResult = await _roleManager.AddClaimAsync(role, permissionName);
            if (!addToRoleResult.Succeeded)
            {
                return BadRequest("error adding user to new role");
            }

            var rolepermit = new RolePermission
            {
                RoleId = role.Id,
                PermissionId = permission
            };
            ctx.RolePermissions.Update(rolepermit);
            await ctx.SaveChangesAsync();
            
            return Ok("Role permission updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePermissionToRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound("Role Not Found");
            }

            var permissionClaims = await _roleManager.GetClaimsAsync(role);
            if (permissionClaims == null)
            {
                return NotFound("Permission not found");
            }

            foreach (var claim in permissionClaims)
            {
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (!result.Succeeded)
                {
                    return BadRequest("Failed to remove permission from role");
                    
                }                
            }
            return Ok("Permission removed from role");
        }
    }
}
