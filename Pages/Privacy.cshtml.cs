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


        //POST method for receiving pin data and saving to file.
        /*public IActionResult OnPost(string owner, string latitude, string longitude, string title, string description, string imageLink)
        {
            Console.WriteLine("reached onpostthroughAJAX");

            double lat = Convert.ToDouble(latitude, CultureInfo.InvariantCulture);
            double lon = Convert.ToDouble(longitude, CultureInfo.InvariantCulture);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "pinLocations.json");
            List<MapPin> mapPins = new List<MapPin>();
            MapPin mapPin = new MapPin(owner, lat, lon, title, description, "placeholder_imagelink");

            //Get all pins from file and deserialize from JSON to list of MapPin objects.
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                mapPins = JsonConvert.DeserializeObject<List<MapPin>>(jsonData) ?? new List<MapPin>();
            }

            //Add new pin to list and write all back to file.
            mapPins.Add(new MapPin(owner,lat,lon,title,description,imageLink));
            string newJsonData = JsonConvert.SerializeObject(mapPins, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, newJsonData);

            return new JsonResult(mapPins);
        }*/


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

        public void OnPostSaveMapView([FromBody] User LoggingOutUser)
        {

            Console.WriteLine("savemapview!");
            Console.WriteLine(LoggingOutUser.Username);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
            List<User> users = new List<User>();

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                users = JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();
            }

            var savedUser = users.FirstOrDefault(u => u.Username == LoggingOutUser.Username);

            if (savedUser != null)
            {
                // Update the user's properties
                savedUser.ViewLatitude = LoggingOutUser.ViewLatitude;
                savedUser.ViewLongitude = LoggingOutUser.ViewLongitude;
                savedUser.ViewZoomLevel = LoggingOutUser.ViewZoomLevel;
            }

            // Serialize the updated list back to JSON
            var updatedJsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, updatedJsonData);

        }

        //GET method for retriving Pin data.
        /*public void OnGetPinData(string id)
        {
            Console.WriteLine("Reached OnGetPinData method");

            //TODO Get real pindata here

            PinTitle = "PinTitle";
            PinDescription = "PinDesc";
        }*/


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

        public IActionResult OnGetUserView(string loggedinUser)
        {
            Console.WriteLine("Trying to get UserView");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
            List<User> users = new List<User>();

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                users = JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();
            }

            var savedUser = users.FirstOrDefault(u => u.Username == loggedinUser);
            User userViewData = new User();

            if (savedUser != null)
            {
                // Update the user's properties
                userViewData.ViewLatitude = savedUser.ViewLatitude;
                userViewData.ViewLongitude = savedUser.ViewLongitude;
                userViewData.ViewZoomLevel = savedUser.ViewZoomLevel;
                Console.WriteLine($"Sending: {userViewData}");
            }

            
            return new JsonResult(userViewData);
        }
    }
}
