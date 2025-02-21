﻿using Microsoft.AspNetCore.Identity;

namespace Kavifx.API.Data
{
    public class AppUser:IdentityUser<int>
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

    public class AppRole:IdentityRole<int>
    {
        public bool IsDeleted { get; set; } = false;
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
    
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<RolePermission> RolePermissions { get; set; }
    }

    public class RolePermission
    {
        public int RoleId { get; set; }
        public AppRole Role { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }

}
