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
    }
    
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RolePermission
    {
        [Key]
        public int Id { get; set; }
        public string RoleId { get; set; }        
        public int PermissionId { get; set; }
        [ForeignKey("RoleId")]
        public virtual AppRole Role { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }

}
