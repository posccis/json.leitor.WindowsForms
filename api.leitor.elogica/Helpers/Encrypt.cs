using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace api.leitor.elogica.Helpers
{
    public class Encrypt
    {
        public Encrypt()
        {

        }
        public string Text {get; set;}
        string hash = "f0xle@rn";
        private object txtEncrypt;

        public string EncryptFunc()
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(Text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using(TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider()
                {
                    Key = keys,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    string encrypted = Convert.ToBase64String(results, 0, results.Length);
                    return encrypted;
                }
            }
        }

        public string DesencryptFunc()
        {
            byte[] data = Convert.FromBase64String(Text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider()
                {
                    Key = keys,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    string decrypted = UTF8Encoding.UTF8.GetString(results);
                    return decrypted;
                }
            }
        }
    }
}
