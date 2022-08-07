using System.Security.Cryptography;
using System.Text;

namespace Tweet.Utils
{
    public class PasswordManager
    {

        public static bool ComparePasswords(string candidatePassword, string hashedPassword)
        {
            var candidateHash = HashPassword(candidatePassword);
            if (candidateHash == hashedPassword) return true;
            return false;
        }
        public static string HashPassword(string password)

        {

            var sha = SHA256.Create();
            var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            //put to string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashedBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();

        }
    }
}