using System.Text;

namespace seaway.API.Configurations
{
    public static class PasswordHelper
    {
        public static string secretKey = "$E@waYH0te!";

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
