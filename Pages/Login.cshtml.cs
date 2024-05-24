using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

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

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
                var jsonData = System.IO.File.ReadAllText(filePath);
                var users = JsonConvert.DeserializeObject<List<User>>(jsonData);

                var matchingUser = users?.FirstOrDefault(user => user.Username == Username);
                if (matchingUser != null && PasswordHelper.VerifyPassword(Password, matchingUser.PasswordHash, matchingUser.Salt))
                {
                    // Korrekt inmatning
                    return RedirectToPage("/Privacy");
                }

                // Fel inmatning
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            // Hantera fall där användarnamn eller lösenord är tomma
            ModelState.AddModelError(string.Empty, "Username and Password are required.");
            return Page();
        }
    }
}
