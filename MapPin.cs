﻿namespace WebApplication1testingRazor
{
    public class MapPin
    {   
        public string Id { get; set; }
        public string Owner { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<UserRating> Ratings{ get; set; } = new List<UserRating>();
        public string ImageLink { get; set; }

        public MapPin() { }

        public MapPin(string owner, double latitude, double longitude, string title, string description, string imageLink)
        {   
            Owner = owner;
            Longitude = longitude;
            Latitude = latitude;
            Title = title;
            Description = description;
            ImageLink = imageLink;
            Id = GenerateUniqueId();
        }

        public override string ToString()
        {
            return $"Id: {Id}, Owner: {Owner}, Title: {Title}, Description: {Description}, Latitude: {Latitude}, Longitude: {Longitude}, ImageLink: {ImageLink}";
        }

        static string GenerateUniqueId()
        {
            // Use the current timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            // Generate a random number
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);

            // Combine the timestamp and random number to create a unique identifier
            return $"{timestamp}{randomNumber}";
        }

        public class UserRating
        {
            public string Username { get; set; }
            public int Rating { get; set; }

            public UserRating(string username, int rating)
            {
                Username = username;
                Rating = rating;
            }
        }
    }
}
