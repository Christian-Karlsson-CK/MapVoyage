namespace WebApplication1testingRazor
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public User(string userId, string username, string passwordHash, string salt)
        {
            UserId = userId;
            Username = username;
            PasswordHash = passwordHash;
            Salt = salt;
        }

        public override string ToString()
        {
            return $"UserId: {UserId}, Username: {Username}, PasswordHash: {PasswordHash}, Salt: {Salt}";
        }
    }
}