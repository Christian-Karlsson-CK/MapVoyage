using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace WebApplication1testingRazor.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        //POST method for receiving pin data.
        public IActionResult OnPost(string latitude, string longitude, string title, string description)
        {
            double lat = Convert.ToDouble(latitude, CultureInfo.InvariantCulture);
            double lon = Convert.ToDouble(longitude, CultureInfo.InvariantCulture);

            //TODO: Process incoming data(arguments) and save to a dataclass.


            //_logger.LogInformation("Received coordinates: Latitude = {Latitude}, Longitude = {Longitude}", latitude, longitude);
            Console.WriteLine($"Received coordinates (string): Latitude = {latitude}, Longitude = {longitude}");
            Console.WriteLine($"Received coordinates (double): Latitude = {lat}, Longitude = {lon}");
            Console.WriteLine($"Received title : {title}");
            Console.WriteLine($"Received description : {description}");

            return Page();
        }
    }
}
