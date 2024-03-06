using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kavifx.API.Data
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
        [Required,DataType(DataType.Password)]
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
        [DataType(DataType.Text)]
        public string Company { get; set; }
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }
        [DataType(DataType.Text)]
        public string Address { get; set; }
        [DataType(DataType.Text)]
        public string City { get; set; }
        [DataType(DataType.Text)]
        public string State { get; set; }
        [DataType(DataType.PostalCode)]
        public string PinCode { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public IFormFile file { get; set; }
    }
    public class UpdateProfilePictureViewModel
    {
        public byte[] PicData { get; set; }
        public string PictureUrl { get; set; }
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
    public class UpdateUserInRoleViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }

    public class UserRoleViewModel
    {
        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> Users { get; set; }
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
        public int TotalUsers { get; set; }
        public int TotalRoles { get; set; }
        public int TotalActiveUsers { get; set; }
        public int TotalDeletedUsers { get; set; }
        public int TotalAssignedRoles { get; set; }        
    }
    #endregion
}
