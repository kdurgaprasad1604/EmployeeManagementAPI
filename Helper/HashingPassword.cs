﻿using System.Security.Cryptography;
namespace EmployeeManagementAPI.Helper
{
    public class HashingPassword
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly int saltSize = 16;
        private static readonly int hashSize = 20;
        private static readonly int iterations = 10000;

        public static string HashPassword(string password)
        {
            byte[] salt; 
            rng.GetBytes(salt = new byte[saltSize]);
            var key = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = key.GetBytes(hashSize);
            var hashBytes = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);
            var base64HashPassword = Convert.ToBase64String(hashBytes);
            return base64HashPassword;
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            var hashBytes = Convert.FromBase64String(base64Hash);
            var salt = new byte[saltSize];
            Array.Copy(hashBytes,0,salt,0, saltSize);
            var key = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = key.GetBytes(hashSize);
            for (int i = 0; i < hashSize; i++)
            {
                if(hashBytes[i + saltSize] != hash[i])
                    return false;
            }
            return true;
        }

    }
}
