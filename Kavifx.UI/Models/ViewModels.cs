using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kavifx.UI.Models
{
    #region Authentication
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }

    #endregion

    #region User
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Profile Picture")]
        public string ProfilePictureUrl { get; set; }
    }

    public class UpdateUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }       
    }

    public class ProfileViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class UpdateProfileViewModel
    {
        [Required,DataType(DataType.Text)]
        public string Company { get; set; }
        [Required, DataType(DataType.Date)]
        public string DateOfBirth { get; set; }
        [Required,DataType(DataType.Text)]
        public string Address { get; set; }
        [Required, DataType(DataType.Text)]
        public string City { get; set; }
        [Required, DataType(DataType.Text)]
        public string State { get; set; }
        [Required,DataType(DataType.PostalCode)]
        public string PinCode { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public IFormFile file { get; set; }
    }

    public class UpdateProfilePictureViewModel
    {
        public IFormFile ProfilePicture { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    #endregion

    #region Role
    public class RoleViewModel
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
    }

    public class CreateRoleViewModel
    {
        public string Name { get; set; }
    }

    public class UpdateRoleViewModel
    {
        public string Name { get; set; }
    }
    #endregion

    #region UserRole
    public class UserInRoleViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }
    public class UserRoleViewModel
    {
        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> Users { get; set; }
        public string SelectedUser { get; set; }
        public string SelectedRole { get; set; }
    }
    public class CreateUserToRoleViewModel
    {
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string SelectedUser { get; set; }
        public string SelectedRole { get; set; }
    }
    public class UpdateUserInRoleViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string SelectedRole { get; set; }
    }

    public class AssignUserToRoleViewModel
    {
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
    #endregion

    #region DashBoard

    public class DashBoardViewModel
    {
        [DisplayName("Users")]
        public int TotalUsers { get; set; }
        [DisplayName("Roles")]
        public int TotalRoles { get; set; }
        [DisplayName("Active Users")]
        public int TotalActiveUsers { get; set; }
        [DisplayName("InActive Users")]
        public int TotalDeletedUsers { get; set; }
        [DisplayName("Assigned Users")]
        public int TotalAssignedRoles { get; set; }
    }
    #endregion
}
