using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class SecurityHelper
    {
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        /// <summary>
        /// 公钥私钥信息
        /// </summary>
        public static readonly string key = "<RSAKeyValue><Modulus>tXD+BESPJ4fmhXvnxdxvTszWb/aSKnsICUcWqMf/4M2q22jIcWzxUcrZyZoszii5GCsbO1KuWXhHASvmrmI1C7OELDr4U/SraOCGr8+e/Sh4XzkDjyWq/0LmC3v0JPOo9bM92ZaDa0mFrnxSwAPY3iSat2evHto4IhIgBZkTXL8=</Modulus><Exponent>AQAB</Exponent><P>7HIYskmXkk560kZPo05PKltur1sQvWEg2f52UkHK6Br6SVUcbvzmXemKzL+ZhvUEOeC4pNih0YWf7rqhziLKgQ==</P><Q>xHJfmjUTKQJNryIcGiNiYgBOLfxe6vk2GB48JkeXuRMccoII7148RyJYSNLLArEepMDpLnwBAhQHTHvoqfoHPw==</Q><DP>vfzgA0JG3HTbE+MTUrE1w188jQKrbMCC2Scyg94B4Ibs3cfZ1QS5RnTF5sd94Yc3IhqDw1GelPC+FeE46p3iAQ==</DP><DQ>IpN67jpvP+WO5MddKOCXfWZOXFuyHSt18PLJZXduZf3OIP1wMylj9KU/4rlvT+761Ma7hBoBV2tNCZI5lklSow==</DQ><InverseQ>jiI30UoHc0qODB+DzDYYw1tDiclfbUwh9kl/V/OYDzVx/eOeL5JQhMDBopPFcgsO0/ik77J9aU1KMCnRUdgYZA==</InverseQ><D>UUPHcc/nMjRrKjQhzfv6GpgiH0mXk9FA+y7M1lGlqGFVeioHRM4fk5vAScx07u1MYafE7aANmOMHIl4wVsCDsvUT5HoWIZYfoh6YJrLheLD2Hj3i7MOq4Yg1dlnScjCQ+dZjOKrCWC7aoiBmXVJw8HHZqzGkCVLJnoktT8mmwwE=</D></RSAKeyValue>";

        /// 解密一个String使用RSA
        /// </summary>
        /// <param name="input">被解密的对象</param>
        /// <param name="encodingUsing">编码格式</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptRAS(string input, Encoding encodingUsing = null)
        {
            //return input;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                encodingUsing = encodingUsing ?? Encoding.UTF8;
                rsa.FromXmlString(key);
                var encryptedBytes = rsa.Decrypt(Convert.FromBase64String(input), false);
                rsa.Clear();
                return encodingUsing.GetString(encryptedBytes);
            }
        }

        /// <summary>
        /// rsa 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodingUsing"></param>
        /// <returns></returns>
        public static string EncryptRAS(string input, Encoding encodingUsing = null)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                encodingUsing = encodingUsing ?? Encoding.UTF8;
                byte[] cipherbytes;
                rsa.FromXmlString(key);
                cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(input), false);
                rsa.Clear();
                return Convert.ToBase64String(cipherbytes);
            }
        }

        public static string MD5(string sourceString)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            // Convert the input string to a byte array and compute the hash.  
            char[] temp = sourceString.ToCharArray();
            byte[] buf = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                buf[i] = (byte)temp[i];
            }
            byte[] data = md5Hasher.ComputeHash(buf);
            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data   
            // and format each one as a hexadecimal string.  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.  
            return sBuilder.ToString().ToUpper();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SHA1(string str)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(str);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }
    }
}
