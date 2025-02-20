using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Business.Exceptions;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment1.Data.Models;
using PRN222.Assignment1.Models;

namespace PRN222.Assignment1.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Register() 
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
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

                return RedirectToAction("Index", "Event");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View();
            }
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserDto
                {
                    Username = registerViewModel.Username,
                    Password = registerViewModel.Password,
                    Fullname = registerViewModel.Fullname,
                    Email = registerViewModel.Email
                };

                try
                {
                    await _userService.RegisterAsync(userDto);
                    return RedirectToAction("Login", "Auth");
                }
                catch (UsernameAlreadyExistException ex)
                {
                    ModelState.AddModelError("Username", ex.Message);
                }
                catch (EmailAlreadyExistException ex)
                {
                    ModelState.AddModelError("Email", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred. Please try again!");
                }
            }
            return View(registerViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
