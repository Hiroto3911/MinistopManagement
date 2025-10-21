using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public class HashPasswordSHA256
    {
         public static string Hash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
          
        }
        public static bool VerifyPassword(string password, string storeHash)
        {
            return Hash(password) == storeHash;
        }
    }
}
