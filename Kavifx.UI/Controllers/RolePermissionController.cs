using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Kavifx.UI.Models;

namespace Kavifx.UI.Controllers
{
    public class RolePermissionController : Controller
    {
        HttpClient client;
        public RolePermissionController(IHttpClientFactory factory)
        {
            client = factory.CreateClient("ApiClient");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("RolePermission/GetRolePermissions");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<PermissionInRoleViewModel>>(json);
                return View(data);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("RolePermission");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<RolePermissionViewModel>(json);
                return View(data);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RolePermissionViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            AssignRolePermissionViewModel rolePermission = new AssignRolePermissionViewModel();
            rolePermission.RoleName = model.SelectedRole;
            rolePermission.PermissionName = model.SelectedPermission;
            var Request = JsonContent.Create(rolePermission);
            var response = await client.PostAsync("RolePermission", Request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "RolePermission");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("RolePermission/GetRolePermissions/" + id);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<PermissionInRoleViewModel>(json);
                return View(data);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);            
            var response = await client.DeleteAsync("RolePermission/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "RolePermission");
            }
            return View();
        }
    }
}
