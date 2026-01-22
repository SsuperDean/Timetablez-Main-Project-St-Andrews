using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetablez.Classes
{
    public class Ciphers
    {
        public static string VernamEncrypt(string text, string key)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char encryptedChar = (char)(text[i] ^ key[i % key.Length]);
                result.Append(encryptedChar);
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(result.ToString()));
        }

        public static string VernamDecrypt(string base64Cipher, string key)
        {
            string cipherText = Encoding.UTF8.GetString(Convert.FromBase64String(base64Cipher));
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < cipherText.Length; i++)
            {
                char decryptedChar = (char)(cipherText[i] ^ key[i % key.Length]);
                result.Append(decryptedChar);
            }
            return result.ToString();
        }

    }
}
