using System.Security.Cryptography;

namespace authentication.API.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 10000; // PBKDF2 iterations

        public void HashPassword(string password, out string hasedSalt, out string hashedPassword)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(KeySize);

            hasedSalt = Convert.ToBase64String(salt);
            hashedPassword = Convert.ToBase64String(hash);
            // Return both salt and hash as base64, joined by delimiter
            //return $"{Convert.ToBase64String(salt)}{Delimiter}{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string hasedSalt, string hashedPassword)
        {
            byte[] salt = Convert.FromBase64String(hasedSalt);
            byte[] storedPasswordHash = Convert.FromBase64String(hashedPassword);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] computedHash = pbkdf2.GetBytes(KeySize);

            // Compare both byte arrays securely (constant-time comparison)
            return CryptographicOperations.FixedTimeEquals(computedHash, storedPasswordHash);
        }
    }
}
