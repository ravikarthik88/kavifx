using Kavifx.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Kavifx.UI.Controllers
{
    public class DashBoardController : Controller
    {
        HttpClient client;
        public DashBoardController(IHttpClientFactory factory)
        {
            client = factory.CreateClient("ApiClient");            
        }
        public async Task<IActionResult> Index()
        {
            string Token = HttpContext.Session.GetString("JWTToken");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync("Dashboard/totals");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<DashBoardViewModel>(json);
                return View(data);
            }
            return View();
        }
    }
}
