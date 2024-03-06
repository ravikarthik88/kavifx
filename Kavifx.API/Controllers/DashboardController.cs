using Kavifx.API.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Kavifx.API.Controllers
{
    [EnableCors("CrossPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("totals")]
        public IActionResult GetTotals()
        {
            var totalUsers = _context.Users.Count();
            var totalRoles = _context.Roles.Count();
            var totalActiveUsers = _context.Users.Count(u => u.IsActive);
            var totalDeletedUsers = _context.Users.Count(u => u.IsDeleted);
            var totalAssignedRoles = _context.UserRoles.Count();

            var stats = new DashBoardViewModel
            {
                TotalUsers = totalUsers,
                TotalRoles = totalRoles,
                TotalActiveUsers = totalActiveUsers,
                TotalDeletedUsers = totalDeletedUsers,
                TotalAssignedRoles = totalAssignedRoles
            };
            return Ok(stats);
        }
    }
}
