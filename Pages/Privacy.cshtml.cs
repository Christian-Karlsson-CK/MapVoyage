using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace WebApplication1testingRazor.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        //Variables used when a pin is clicked to store its retrieved data.
        public string PinTitle { get; private set; }
        public string PinDescription { get; private set; }

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        //POST method for receiving pin data.
        public IActionResult OnPost(string owner, string latitude, string longitude, string title, string description)
        {
            double lat = Convert.ToDouble(latitude, CultureInfo.InvariantCulture);
            double lon = Convert.ToDouble(longitude, CultureInfo.InvariantCulture);

            MapPin mapPin = new MapPin(owner, lat, lon, title, description, "placeholder_imagelink");
            Console.WriteLine($"{mapPin}");

            //_logger.LogInformation("Received coordinates: Latitude = {Latitude}, Longitude = {Longitude}", latitude, longitude);

            return Page();
        }

        //GET method for retriving Pin data.
        public void OnGetPinData(string id)
        {
            Console.WriteLine("Reached OnGetPinData method");

            //TODO Get real pindata here

            PinTitle = "PinTitle";
            PinDescription = "PinDesc";
        }
    }
}
