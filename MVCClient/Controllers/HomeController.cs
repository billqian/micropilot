using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient.Models;

namespace MVCClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRefitTest _api;

        public HomeController(ILogger<HomeController> logger, IRefitTest api)
        {
            _logger = logger;
            _api = api;
        }

        public IActionResult Index()
        {
            try {
                var token = HttpContext.User?.Claims.FirstOrDefault(s => s.Type == ClaimTypes.Sid).Value;
                ViewBag.msg = _api.GetValue("Bearer " + token).Result;
            } catch (Exception ex) {
                ViewBag.msg = ex.Message + ex.StackTrace;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            ViewBag.error = "";
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(string username, string password, string returnUrl = "")
        {
            var ret = _api.Login(username, password).Result;
            ViewBag.error = "";
            if (ret.Status) {
                var token = ret.Token;
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Sid, token));
                claims.Add(new Claim(ClaimTypes.Name, ret.Name));
                
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(new ClaimsIdentity(claims, "form")));
                
                return new RedirectResult(string.IsNullOrEmpty(returnUrl) ? "~/" : returnUrl);

            } else {
                ViewBag.error = "Incorrect login name or password.";
                return View();
            }
        }

    }
}
