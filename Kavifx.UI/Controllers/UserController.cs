using Kavifx.UI.helper;
using Kavifx.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Kavifx.UI.Controllers
{
    public class UserController : Controller
    {
        HttpClient client;
        public UserController(IHttpClientFactory factory)
        {
            client = factory.CreateClient("ApiClient");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("User");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<UserViewModel>>(json);
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
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var ReqContent = JsonContent.Create(model);
            var response = await client.PostAsync("Auth/Register",ReqContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpGet]       
        public async Task<IActionResult> Edit(string id)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("User/"+id);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UpdateUserViewModel>(json);
                return View(data);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id,UpdateUserViewModel model)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);            
            var ReqContent = JsonContent.Create(model);
            var response = await client.PutAsync("User/"+id, ReqContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("User/"+id);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserViewModel>(json);
                return View(data);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.DeleteAsync("User/"+id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }
    }
}
