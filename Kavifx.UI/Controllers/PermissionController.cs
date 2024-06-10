using Kavifx.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Kavifx.UI.Controllers
{
    public class PermissionController : Controller
    {
        HttpClient client;
        public PermissionController(IHttpClientFactory factory)
        {
            client = factory.CreateClient("ApiClient");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("Permission");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<PermissionViewModel>>(json);
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
        public async Task<IActionResult> Create(PermissionViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var ReqContent = JsonContent.Create(model);
            var response = await client.PostAsync("Permission", ReqContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Permission");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, PermissionViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var ReqContent = JsonContent.Create(model);
            var response = await client.PutAsync("Permission/"+id, ReqContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Permission");
            }
            return View();
        }
    }
}
