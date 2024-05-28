using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OsmSharp.API;
using System.Globalization;


namespace WebApplication1testingRazor.Pages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
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
            // Kontrollera om användaren är inloggad
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name; // Hämtar användarnamnet från claims
                TempData["Username"] = username;
                _logger.LogInformation($"Användaren {username} är inloggad.");
            }
            else
            {
                _logger.LogInformation("Användaren är inte inloggad.");
            }
        }


        public void OnPost([FromBody] MapPin pinData)
        {
            
            var user = User.Identity.Name;
            double lat = Convert.ToDouble(pinData.Latitude, CultureInfo.InvariantCulture);
            double lon = Convert.ToDouble(pinData.Longitude, CultureInfo.InvariantCulture);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "pinLocations.json");
            List<MapPin> mapPins = new List<MapPin>();
            MapPin mapPin = new MapPin(user, lat, lon, pinData.Title, pinData.Description, pinData.ImageLink);

            //Get all pins from file and deserialize from JSON to list of MapPin objects.
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                mapPins = JsonConvert.DeserializeObject<List<MapPin>>(jsonData) ?? new List<MapPin>();
            }

            //Add new pin to list and write all back to file.
            mapPins.Add(mapPin);
            string newJsonData = JsonConvert.SerializeObject(mapPins, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, newJsonData);

        }


        public IActionResult OnGetAllPinData()
        {
            Console.WriteLine("Reached OnGetAllPinData method");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "pinLocations.json");
            List<MapPin> mapPins = new List<MapPin>();

            //Get all pins from file and deserialize from JSON to list of MapPin objects.
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                mapPins = JsonConvert.DeserializeObject<List<MapPin>>(jsonData) ?? new List<MapPin>();
            }

            return new JsonResult(mapPins);
        }

        public IActionResult OnPostRemovePin([FromBody] RemovePinRequest removePinRequest)
        {
            var user = User.Identity.Name;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "pinLocations.json");
            List<MapPin> mapPins = new List<MapPin>();

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                mapPins = JsonConvert.DeserializeObject<List<MapPin>>(jsonData) ?? new List<MapPin>();
            }

            var pinToRemove = mapPins.FirstOrDefault(p => p.Owner == user && p.Latitude == removePinRequest.Latitude && p.Longitude == removePinRequest.Longitude);
            if (pinToRemove != null)
            {
                mapPins.Remove(pinToRemove);
                string newJsonData = JsonConvert.SerializeObject(mapPins, Formatting.Indented);
                System.IO.File.WriteAllText(filePath, newJsonData);
                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false, message = "Pin not found or you do not have permission to remove it." });
        }


        public class RemovePinRequest
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
