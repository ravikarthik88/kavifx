using Kavifx.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Kavifx.UI.Controllers
{
    public class UserRoleController : Controller
    {
        HttpClient client;
        public UserRoleController(IHttpClientFactory factory)
        {
            client = factory.CreateClient("ApiClient");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("UserRole/GetUserRoles");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<UserInRoleViewModel>>(json);
                return View(data);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("UserRole");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserRoleViewModel>(json);
                return View(data);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRoleViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            AssignUserToRoleViewModel assignUserToRole = new AssignUserToRoleViewModel();
            assignUserToRole.Email = model.SelectedUser;
            assignUserToRole.RoleName = model.SelectedRole;
            var Request = JsonContent.Create(assignUserToRole);
            var response = await client.PostAsync("UserRole", Request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "UserRole");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("UserRole/"+id);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UpdateUserInRoleViewModel>(json);
                return View(data);
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, UpdateUserInRoleViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            AssignUserToRoleViewModel assignUserToRole = new AssignUserToRoleViewModel();           
            assignUserToRole.Email = model.Email;
            assignUserToRole.RoleName = model.SelectedRole;
            var Request = JsonContent.Create(assignUserToRole);
            var response = await client.PutAsync("UserRole/"+id, Request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "UserRole");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.DeleteAsync("UserRole/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
    }
}
