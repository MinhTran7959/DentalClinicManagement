
using System;
using System.Security.Cryptography;
using System.Text;

namespace QLRHM7.ViewModel
{
    public class SHA512Encryption
    {
        public string Encrypt(string plainText)
        {
            if (plainText.Equals(""))
            {
                return "";
            }
            else
            {
                using (SHA512 sha = SHA512.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] hash = sha.ComputeHash(bytes);

                    return Convert.ToBase64String(hash);
                }
            }

        }

        public bool Verify(string plainText, string hash)
        {
            if (hash.Equals(""))
            {
                return false;
            }
            else
            {
                string hashOfInput = Encrypt(plainText);
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                return comparer.Compare(hashOfInput, hash) == 0;
            }

        }
        public string Descrypt(string plainText, string hash)
        {
            if (hash.Equals(""))
            {
                return "";
            }
            else
            {
                string hashOfInput = Encrypt(plainText);

                return hashOfInput;
            }

        }

    }
}
