using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Business.Exceptions;
using PRN222.Assignment1.Business.Services;
using PRN222.Assignment2.Models;

namespace PRN222.Assignment2.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        public readonly IUserService _userService;
        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("Event/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Event/Index");
            }

            var userDto = new UserDto
            {
                Username = RegisterViewModel.Username,
                Password = RegisterViewModel.Password,
                Email = RegisterViewModel.Email,
                Fullname = RegisterViewModel.Fullname
            };

            try
            {
                await _userService.RegisterAsync(userDto);
                return RedirectToPage("/Auth/Login");
            }
            catch (UsernameAlreadyExistException e)
            {
                ModelState.AddModelError("Username", e.Message);
            }
            catch (EmailAlreadyExistException e)
            {
                ModelState.AddModelError("Email", e.Message);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return Page();
        }   
    }
}
