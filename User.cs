namespace WebApplication1testingRazor
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override string ToString()
        {
            return $"Username: {Username}, Password: {Password}";
        }
    }
}
