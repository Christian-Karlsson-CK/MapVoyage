using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApplication1testingRazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string category, [FromQuery] string query)
        {
            // Example data - you should replace this with your actual data source
            var locations = new List<Location>
            {
                new Location { Name = "London", Type = "City", Lat = 51.5074, Lng = -0.1278 },
                new Location { Name = "Paris", Type = "City", Lat = 48.8566, Lng = 2.3522 },
                new Location { Name = "The Fat Duck", Type = "Restaurant", Lat = 51.5112, Lng = -0.1392 }
            };

            // Filter the locations based on the category and query
            var filteredLocations = locations.FindAll(location =>
                (category == "all" || location.Type.ToLower() == category.ToLower()) &&
                location.Name.ToLower().Contains(query.ToLower()));

            return Ok(new { locations = filteredLocations });
        }
    }
    public class Location
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
