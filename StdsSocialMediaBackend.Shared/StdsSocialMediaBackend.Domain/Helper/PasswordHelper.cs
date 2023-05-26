using System.Security.Cryptography;
using System.Text;

namespace StdsSocialMediaBackend.Domain.Helper
{
    public static class PasswordHelper
    {
        //ToDO: Salt statt Hash

        public static string HashPw(string password)
        {
            var sha = SHA256.Create();
            var ba = Encoding.Default.GetBytes(password);
            var hashed = sha.ComputeHash(ba);
            return Convert.ToBase64String(hashed);
        }
    }
}
