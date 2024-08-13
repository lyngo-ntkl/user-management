using System.Security.Cryptography;

namespace UserManagement.Infrastructure.Common
{
    public class HashingHelper
    {
        private const int SaltBytes = 16;
        private const int HashBytes = 32;
        private const int Iteration = 10000;
        private static HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
        public static void Hash(string value, out byte[] hash, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(SaltBytes);
            var pbkdf2 = new Rfc2898DeriveBytes(value, salt, Iteration, Algorithm);
            hash = pbkdf2.GetBytes(HashBytes);
        }

        public static void Hash(string value, byte[] salt, out byte[] hash)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(value, salt, Iteration, Algorithm);
            hash = pbkdf2.GetBytes(HashBytes);
        }

        public static void Hash(string value, out string hash, out string salt)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltBytes);
            var pbkdf2 = new Rfc2898DeriveBytes(value, saltBytes, Iteration, Algorithm);
            byte[] hashBytes = pbkdf2.GetBytes(HashBytes);
            hash = Convert.ToBase64String(hashBytes);
            salt = Convert.ToBase64String(saltBytes);
        }

        public static void Hash(string value, string salt, out string hash)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(value, saltBytes, Iteration, Algorithm);
            byte[] hashBytes = pbkdf2.GetBytes(HashBytes);
            hash = Convert.ToBase64String(hashBytes);
        }
    }
}
