using Kavifx.UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Kavifx.UI.Controllers
{ 
    public class AccountController : Controller
    {
        HttpClient client;
        private readonly ILogger<AccountController> _logger;
        private readonly IWebHostEnvironment _env;

        public AccountController(IHttpClientFactory factory,
            ILogger<AccountController> logger,
            IWebHostEnvironment environment)
        {
            client = factory.CreateClient("ApiClient");
            _logger = logger;
            _env = environment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            List<Claim> authclaims = new List<Claim>();
            LoginViewModel login = new LoginViewModel()
            {
                Email = model.Email,
                Password = model.Password
            };

            var ReqContent = JsonContent.Create(login);
            var response = await client.PostAsync("Auth/Login", ReqContent);
            if (response.IsSuccessStatusCode)
            {
                string Token = await response.Content.ReadAsStringAsync();
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(Token) as JwtSecurityToken;
                if (jsonToken != null)
                {
                    authclaims = (List<Claim>)jsonToken.Claims;
                }
                var identity = new ClaimsIdentity(authclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("JWTToken", Token);
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            RegisterViewModel profile = new RegisterViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            };
            var jsonData = JsonContent.Create(profile);
            var responseMessage = await client.PostAsync("Auth/Register", jsonData);
            if (responseMessage.IsSuccessStatusCode)
            {
                _logger.LogInformation(responseMessage.StatusCode + " " + responseMessage.Content);
                return RedirectToAction("Login", "Account");

            }
            else
            {
                _logger.LogInformation($"Failed to upload profile picture. Status code: {responseMessage.StatusCode}");
                return RedirectToAction("Register", "Account");
            }

        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
