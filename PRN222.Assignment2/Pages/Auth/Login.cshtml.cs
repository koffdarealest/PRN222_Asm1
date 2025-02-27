using System.Dynamic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment2.Models;
using System.Security.Claims;

namespace PRN222.Assignment2.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public readonly IUserService _userService;
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("Event/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Event/Index");
            }

            var user = await _userService.AuthenticationAsync(loginViewModel.Username, loginViewModel.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToPage("/Event/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return Page();
            }
        }
    }
}
