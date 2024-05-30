namespace WebApplication1testingRazor
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public double ViewLatitude { get; set; }
        public double ViewLongitude { get; set; }
        public int ViewZoomLevel { get; set; }
        public List<string> FavoriteList { get; set; }

        public User() { }

        public User(string userId, string username, string passwordHash, string salt)
        {
            UserId = userId;
            Username = username;
            PasswordHash = passwordHash;
            Salt = salt;
            ViewLatitude = 58.0;
            ViewLongitude = 13.0;
            ViewZoomLevel = 13;
            FavoriteList = new List<string>();
        }

        public override string ToString()
        {
            return $"UserId: {UserId}, Username: {Username}, PasswordHash: {PasswordHash}, Salt: {Salt}, ViewLatitude: {ViewLatitude}, ViewLongitude: {ViewLongitude}, ViewZoomLevel: {ViewZoomLevel}";
        }
    }
}