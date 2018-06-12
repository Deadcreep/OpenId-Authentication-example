using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WebApplication1.Extensions;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpPost]
        public async Task<IActionResult> ULogin()
        {

            string link = string.Format("http://ulogin.ru/token.php?token={0}&host={1}", Request.Form["token"], Request.Host);
            Console.WriteLine(link);
            WebRequest reqGET = WebRequest.Create(link);
            string answer = "";
            
            using (WebResponse resp = reqGET.GetResponse())
            {
                using (Stream stream = resp.GetResponseStream())
                {
                    if (stream != null)
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            answer = sr.ReadToEnd();
                            Console.WriteLine(answer);
                        }
                }
            }
            if (!string.IsNullOrWhiteSpace(answer))
            {
                var user = JSONHelper.Deserialise<ULoginUser>(answer);
                if(user != null)
                {
                        var appUser = new ApplicationUser
                        {
                            Email = user.Email,
                            UserName = $"{user.FirstName} {user.LastName}",
                            SecurityStamp = user.Uid,
                        };
                    
                    await _signInManager.SignInAsync(appUser, true);                     
                }
            }
            
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        
        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
