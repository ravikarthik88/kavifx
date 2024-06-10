using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Kavifx.API.Data
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public DateTime DateOfBirth { get; set; }        
        public string PictureUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }

    public class AppRole:IdentityRole
    {
        public bool IsDeleted { get; set; } = false;
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
    
    public class Permission
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<RolePermission> RolePermissions { get; set; }
    }

    public class RolePermission
    {
        public string RoleId { get; set; }
        public AppRole Role { get; set; }

        public string PermissionId { get; set; }
        public Permission Permission { get; set; }
    }

}
