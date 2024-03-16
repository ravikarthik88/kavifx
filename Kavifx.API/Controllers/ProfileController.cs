using Kavifx.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kavifx.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]   
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public ProfileController(UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _env = environment;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> ViewProfile(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            var profile = new ProfileViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.PictureUrl == null ? $"{Request.Scheme}://{Request.Host}{Request.PathBase}/uploads/common/avatar.png" : user.PictureUrl
            };
            return Ok(profile);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProfile(string id,[FromForm]UpdateProfileViewModel model)
        {
            string ImageUrl = string.Empty;
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound("User Not Found");
            }

            if(model.file == null || model.file.Length == 0 || Request.Form.Files.Count > 1)
            {
                return BadRequest("Invalid request data");
            }

            var webrootpath = _env.WebRootPath;            
            var filePath = Path.Combine(webrootpath, "uploads", "profilepics");            
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            //check allowed extensions
            var extn = Path.GetExtension(model.file.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(extn))
            {
                return BadRequest(new { message = "Only {0} extensions are allowed", allowedextn = string.Join(",", allowedExtensions) });                
            }

            string uniquestring = Guid.NewGuid().ToString();
            var newFileName = uniquestring + extn;
            var filewithPath = Path.Combine(filePath, newFileName);
            using (var stream = new FileStream(filewithPath, FileMode.Create))
            {
                await model.file.CopyToAsync(stream);
                await stream.FlushAsync();
            }

            if (System.IO.File.Exists(filewithPath))
            {
                 ImageUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/uploads/profilepics/{newFileName}";
            }
            else
            {
                 ImageUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/uploads/common/avatar.png";
            }
            user.PhoneNumber = model.PhoneNumber;
            user.PhoneNumberConfirmed = true;
            user.PictureUrl = ImageUrl;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "file uploaded successfully" });
        }

        [HttpPut("changeprofilepic")]
        public async Task<IActionResult> UpdateProfilePicture([FromForm]UpdateProfilePictureViewModel model)
        {
            string ImageUrl = string.Empty;
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User Not Found");
            }
            if (model.file == null || model.file.Length == 0 || Request.Form.Files.Count > 1)
            {
                return BadRequest("Invalid request data");
            }

            var webrootpath = _env.WebRootPath;
            var filePath = Path.Combine(webrootpath, "uploads", "profilepics");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            //check allowed extensions
            var extn = Path.GetExtension(model.file.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(extn))
            {
                return BadRequest(new { message = "Only {0} extensions are allowed", allowedextn = string.Join(",", allowedExtensions) });
            }

            string uniquestring = Guid.NewGuid().ToString();
            var newFileName = uniquestring + extn;
            var filewithPath = Path.Combine(filePath, newFileName);
            using (var stream = new FileStream(filewithPath, FileMode.Create))
            {
                await model.file.CopyToAsync(stream);
                await stream.FlushAsync();
            }

            if (System.IO.File.Exists(filewithPath))
            {
                ImageUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/uploads/profilepics/{newFileName}";
            }
            else
            {
                ImageUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/uploads/common/avatar.png";
            }
            user.PictureUrl = ImageUrl;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { message = "file uploaded successfully" });            
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password changed successfully.");
        }
    }
}
