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

        public static string EncryptPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            else
            {
                password = password + secretKey;

                var passwordInByte = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(passwordInByte);
            }
        }

        public static string DecryptPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            else
            {
                var encodedByte = Convert.FromBase64String(password);

                var actualPassword = Encoding.UTF8.GetString(encodedByte);
                actualPassword = actualPassword.Substring(0, actualPassword.Length - secretKey.Length);

                return actualPassword;
            }
        }
    }
}
