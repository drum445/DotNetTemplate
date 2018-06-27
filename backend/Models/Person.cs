using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace backend.Models
{
    public class Person
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public void HashPassword()
        {
            // use our personId as the salt as it is a guid
            byte[] salt = Encoding.ASCII.GetBytes(this.Id);
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: this.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            this.Password = hashed;
        }
    }
}
