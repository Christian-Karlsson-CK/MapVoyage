using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebApplication1testingRazor.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        [BindProperty]
        public string? ConfirmPassword { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
            {
                ModelState.AddModelError(string.Empty, "Username, Password and Confirm Password are required.");
                return Page();
            }

            if (!Password.Equals(ConfirmPassword))
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");

            List<User> users;

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                users = JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();
            }
            else
            {
                users = new List<User>();
            }

            if (users.Any(user => user.Username.Equals(Username, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(string.Empty, "Username already exists. Please choose another username.");
                return Page();
            }
            var salt = PasswordHelper.GenerateSalt();
            var hashedPassword = PasswordHelper.HashPassword(Password, salt);

            users.Add(new User(Username, hashedPassword, salt));

            var updatedJsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, updatedJsonData);

            return RedirectToPage("/Login");
        }
    }
}
