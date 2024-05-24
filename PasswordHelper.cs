using System.Security.Cryptography;
using System.Text;
namespace WebApplication1testingRazor
{
    public class PasswordHelper
    {
        public static string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            using ( var sha256 = SHA256.Create()) 
            {
                var combinedPassword = password + salt;
                var bytes = Encoding.UTF8.GetBytes(combinedPassword);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var hashOfEnteredPassword = HashPassword(enteredPassword, storedSalt);
            return hashOfEnteredPassword == storedHash;
        }
    }
}
