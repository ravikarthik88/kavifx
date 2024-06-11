using Kavifx.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kavifx.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly AppDbContext ctx;
        public PermissionController(AppDbContext context)
        {
            ctx = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionViewModel>>> GetAllPermissions()
        {
            var permissions = await ctx.Permissions.Select(x => new PermissionViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();

            return Ok(permissions);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionViewModel>> GetPermissions(string id)
        {
            var permission = await ctx.Permissions.FindAsync(id);
            if(permission == null)
            {
                return NotFound();
            }

            var permissions = new PermissionViewModel
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description
            };

            return Ok(permissions);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionViewModel viewModel)
        {
            var permission = new Permission
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };

            ctx.Permissions.Add(permission);
            await ctx.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(string id, [FromBody] UpdatePermissionViewModel viewModel)
        {
            var permission = await ctx.Permissions.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }

            permission.Name = viewModel.Name;
            permission.Description = viewModel.Description;
            ctx.Permissions.Update(permission);
            await ctx.SaveChangesAsync();
            return Ok(permission);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(string id)
        {
            var permission = await ctx.Permissions.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }

            permission.IsDeleted = true;            
            ctx.Permissions.Update(permission);
            await ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
