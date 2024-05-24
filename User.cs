namespace WebApplication1testingRazor
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public User(string username, string passwordHash, string salt)
        {
            Username = username;
            PasswordHash = passwordHash;
            Salt = salt;
        }

        public override string ToString()
        {
            return $"Username: {Username}, PasswordHash: {PasswordHash}, Salt: {Salt}";
        }
    }
}
