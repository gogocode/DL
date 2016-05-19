using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DL.Web.Utilities
{
    public class MD5Encoder
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string encrypt)
        {
            string ret = string.Empty;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encrypt);

            using (MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider())
            {
                ret = BitConverter.ToString(csp.ComputeHash(inputByteArray)).Replace("-", string.Empty);
            }

            return ret;
        }

    }
}