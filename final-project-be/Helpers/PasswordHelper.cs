using System.Security.Cryptography;
using System.Text;

namespace final_project_be.Helpers
{
    public class PasswordHelper
    {
        public static string EncryptPassword(string rawPassword)
        {
            //From String to byte array
            byte[] sourceBytes = Encoding.UTF8.GetBytes(rawPassword);
            byte[] hashBytes = SHA1.HashData(sourceBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

            return hash;
        }
    }
}