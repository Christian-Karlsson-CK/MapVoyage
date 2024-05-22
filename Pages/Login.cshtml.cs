using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1testingRazor.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnPost()
        {
            // LÄgg till autentiseringslogik
            // Omdirigera nu direkt till Privacy-sidan
            return RedirectToPage("/Privacy");
        }
    }
}
