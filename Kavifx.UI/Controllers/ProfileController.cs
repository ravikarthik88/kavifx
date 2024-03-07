using Kavifx.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace Kavifx.UI.Controllers
{
    public class ProfileController : Controller
    {
        HttpClient client;       

        public ProfileController(IHttpClientFactory factory)
        {
            client = factory.CreateClient("ApiClient");
        }

        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("Profile/"+email);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProfileViewModel>(json);
                return View(data);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id,UpdateProfileViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(model.Company), "Company");
                form.Add(new StringContent(model.DateOfBirth), "DateOfBirth");
                form.Add(new StringContent(model.Location), "Location");                
                form.Add(new StringContent(model.PhoneNumber), "PhoneNumber");
                using var filestream = model.file.OpenReadStream();
                form.Add(new StreamContent(filestream), "file", model.file.FileName);
                var responseMessage = await client.PutAsync("Profile/"+id, form);                
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }            
        }

        [HttpGet]
        public IActionResult ChangePicture()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePicture(UpdateProfilePictureViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(model.UserId), "UserId");
                form.Add(new StreamContent(model.file.OpenReadStream()), "file", model.file.FileName);
                var responseMessage = await client.PutAsync("Profile/changeprofilepic", form);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string email)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("Profile/" + email);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProfileViewModel>(json);
                return View(data);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var ReqContent = JsonContent.Create(model);
            var response = await client.PostAsync("Profile/changepassword", ReqContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }
    }
}
