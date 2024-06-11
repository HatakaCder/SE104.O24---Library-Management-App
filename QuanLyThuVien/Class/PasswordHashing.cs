using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Class
{
    public class PasswordHashing
    {
        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Create a new StringBuilder to collect the bytes
                // and create a string.
                StringBuilder stringBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return stringBuilder.ToString();
            }
        }

        // Example of verifying a password against a hashed password
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            // Hash the input password using the same algorithm and compare it to the stored hash.
            string hashedInputPassword = HashPassword(inputPassword);
            return hashedInputPassword.Equals(hashedPassword);
        }
    }
}
