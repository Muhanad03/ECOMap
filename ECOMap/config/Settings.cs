using ECOMap.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECOMap.config
{
    public class Settings
    {
        public static string MainGreenColor = "#40C057";
        public static string Token_Name = "AuthToken";
        public static bool IsUserLoggedIn = false;
        public static userData? CurrentUser = null;
        /*
         * 
         * user enters the password 
         * password gets hashed on the client side 
         * password gets to the backend api 
         * password gets compared to the hashed password stored in the data base  
         * 
         * if the hashed password gets intersected by an outsider can they login using it?
         * SHA256 hash is not reversable 
         * 
         * Login is now secure enough?? 
         * 
         * Maybe add double hashing in the future
         * 
         * 
         * 
         */



        public static string hashPassword(string passwordToHash)
        {
            if (passwordToHash != null)
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordToHash));

                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in hashedBytes)
                    {
                        builder.Append(b.ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
            else
            {
                return null;
            }
        }


    }
}
