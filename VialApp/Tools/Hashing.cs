using System.Security.Cryptography;
using System.Text;

namespace VialApp.Tools
{
    public static class Hashing
    {
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] stream = sha256.ComputeHash(Encoding.ASCII.GetBytes(str));
            StringBuilder sb = new();
            for (int i = 0; i < stream.Length; i++)
                sb.AppendFormat("{0:x2}", stream[i]);

            return sb.ToString();
        }
    }
}
