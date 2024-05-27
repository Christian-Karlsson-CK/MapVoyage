using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1testingRazor.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
                var jsonData = System.IO.File.ReadAllText(filePath);
                var users = JsonSerializer.Deserialize<List<User>>(jsonData);

                var matchingUser = users?.FirstOrDefault(user => user.Username == Username);
                if (matchingUser != null && PasswordHelper.VerifyPassword(Password, matchingUser.PasswordHash, matchingUser.Salt))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, matchingUser.Username),
                        new Claim(ClaimTypes.NameIdentifier, matchingUser.UserId.ToString())
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddDays(30)
                        });

                    return RedirectToPage("/Privacy");
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            ModelState.AddModelError(string.Empty, "Username and Password are required.");
            return Page();
        }
    }
}
