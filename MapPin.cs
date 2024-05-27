﻿namespace WebApplication1testingRazor
{
    public class MapPin
    {
        public string Owner { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }

        public MapPin(string owner, double latitude, double longitude, string title, string description, string imageLink)
        {
            Owner = owner;
            Longitude = longitude;
            Latitude = latitude;
            Title = title;
            Description = description;
            ImageLink = imageLink;
        }

        public override string ToString()
        {
            return $"Owner: {Owner}, Title: {Title}, Description: {Description}, Latitude: {Latitude}, Longitude: {Longitude}, ImageLink: {ImageLink}";
        }
    }
}