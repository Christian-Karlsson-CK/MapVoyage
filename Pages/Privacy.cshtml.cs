using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OsmSharp.API;
using System.Globalization;
using static WebApplication1testingRazor.MapPin;


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

            //Get all pins from file and deserialize from JSON to list of MapPin objects.
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                mapPins = JsonConvert.DeserializeObject<List<MapPin>>(jsonData) ?? new List<MapPin>();
            }
            
            MapPin mapPin = new MapPin(user, lat, lon, pinData.Title, pinData.Description, pinData.ImageLink)
            {
                Ratings = pinData.Ratings
            };

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
            public double Longitude { get; set;}
        }

        public void OnPostAddRating([FromBody] RatingData ratingData)
        {
            var user = User.Identity.Name;
            double lat = Convert.ToDouble(ratingData.Latitude, CultureInfo.InvariantCulture);
            double lon = Convert.ToDouble(ratingData.Longitude, CultureInfo.InvariantCulture);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "pinLocations.json");
            List<MapPin> mapPins = new List<MapPin>();

            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                mapPins = JsonConvert.DeserializeObject<List<MapPin>>(jsonData) ?? new List<MapPin>();
            }

            var pin = mapPins.FirstOrDefault(p => p.Latitude == lat && p.Longitude == lon);
            if (pin != null)
            {
                var existingRating = pin.Ratings.FirstOrDefault(r => r.Username == user);
                if (existingRating != null)
                {
                    existingRating.Rating = ratingData.Rating;
                }
                else
                {
                    pin.Ratings.Add(new UserRating(user, ratingData.Rating));
                }

                var newJsonData = JsonConvert.SerializeObject(mapPins, Formatting.Indented);
                System.IO.File.WriteAllText(filePath, newJsonData);
            }
        }

        public class RatingData
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public int Rating { get; set; }
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
