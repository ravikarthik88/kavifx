using Kavifx.UI.helper;
using Kavifx.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Kavifx.UI.Controllers
{
    public class RoleController : Controller
    {
        HttpClient client;
        public RoleController(IHttpClientFactory factory)
        {
            client = factory.CreateClient("ApiClient");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("Role");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<RoleViewModel>>(json);
                return View(data);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var ReqContent = JsonContent.Create(model);
            var response = await client.PostAsync("Role", ReqContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Role");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id,UpdateRoleViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var ReqContent = JsonContent.Create(model);
            var response = await client.PutAsync("Role/"+id, ReqContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Role");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("Role/" + id);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<RoleViewModel>(json);
                return View(data);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);            
            var response = await client.DeleteAsync("Role/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Role");
            }
            return View();
        }
    }
}
