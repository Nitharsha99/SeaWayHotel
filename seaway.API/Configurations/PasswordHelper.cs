using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace seaway.API.Configurations
{
    public class PasswordHelper
    {
        private const int _saltSize = 128 / 8;
        private const int _keySize = 256 / 8;
        private const int _iteration = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char _delimiter = ';';

        public string HashingPassword(string password)
        {
            var saltByte = RandomNumberGenerator.GetBytes(_saltSize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(password, saltByte, _iteration, _hashAlgorithmName, _keySize);

            return string.Join(_delimiter, Convert.ToBase64String(saltByte), Convert.ToBase64String(hash));

        }

        public bool Verification(string hashPassword, string inputPassword)
        {
            var elements = hashPassword.Split(_delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, _iteration, _hashAlgorithmName, _keySize);

            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }

    }
}
