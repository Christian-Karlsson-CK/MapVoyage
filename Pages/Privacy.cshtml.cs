using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OsmSharp.API;
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
        public IActionResult OnPost(string owner, string latitude, string longitude, string title, string description, string imageLink)
        {
            double lat = Convert.ToDouble(latitude, CultureInfo.InvariantCulture);
            double lon = Convert.ToDouble(longitude, CultureInfo.InvariantCulture);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "pinLocations.json");

            List<MapPin> mapPins = new List<MapPin>();
            
            MapPin mapPin = new MapPin(owner, lat, lon, title, description, "placeholder_imagelink");
            // Console.WriteLine($"{mapPin}");

            string json = JsonConvert.SerializeObject(mapPins, Formatting.Indented);

            System.IO.File.WriteAllText(filePath, json );

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                mapPins = JsonConvert.DeserializeObject<List<MapPin>>(jsonData) ?? new List<MapPin>();
            }

            foreach (MapPin test in mapPins)
            {
                Console.WriteLine(test);
            }

            mapPins.Add(new MapPin(owner,lat,lon,title,description,imageLink));

            string newJsonData = JsonConvert.SerializeObject(mapPins, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, newJsonData);

            // Console.WriteLine("MapPin data written succesfully!");

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
