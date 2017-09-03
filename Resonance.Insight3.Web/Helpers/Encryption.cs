using System;
using System.Security.Cryptography;
using System.Text;

namespace Resonance.Insight3.Web.Helpers
{
    /// <summary>
    ///     Summary description for Encryption.
    ///     The HashString class has one static method, GetMd5Sum,
    ///     which uses the .NET crypto code to create an md5 has
    ///     of the string you pass in, and then takes the resulting
    ///     bytes and converts them back into a string.  The idea
    ///     is simply to have two strings hash to a very unique value,
    ///     more unique than String.GetHashCode(), for situations where
    ///     you'll be storing a lot of hashed objects in a database and
    ///     32 bits might not be enough of a hash.
    /// </summary>
    public class Encryption
    {
        // Create an md5 sum string of this string
        public static string GetMd5Sum(string str)
        {
            try
            {
                var enc = new UTF8Encoding();

                // Use the CSP to hash the password
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(enc.GetBytes(str.ToCharArray()));
                var sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("X2")); // md5 to Hex
                }
                return sb.ToString();
            }
            catch (Exception err)
            {
                throw new Exception("Encryption.GetMd5Sum: " + err.Message, err.InnerException);
            }
        }
    }
}