using Microsoft.AspNetCore.Identity;

namespace ProjektArbeitDanijelMademidda.Data
{
    public class HashGenerator
    {
        /// <summary>
        /// Generates a new hash according the given password.
        /// </summary>
        /// <param name="password">Password to be hashed.</param>
        /// <param name="salt">Returns the generated salt.</param>
        /// <returns>Returns the generated hash code as string.</returns>
        
        public static string GenerateHash(string password, out string salt)
        {
            var pwHasher = new PasswordHasher<object>();
            salt = Guid.NewGuid().ToString("N");
            return pwHasher.HashPassword(null, password + salt);
        }

        /// <summary>
        /// Verifies that an existing hash corresponds to the password and its salt.
        /// </summary>
        /// <param name="hash">Hash code to be compared.</param>
        /// <param name="password">Password to be compared against the hash.</param>
        /// <param name="salt">Salt create during the hash algorithm.</param>
        /// <returns>Returns true if the given hash machtes the password.</returns>
        /// 

        public static bool VerifyHash(string hash, string password, string salt)
        {
            var pwHasher = new PasswordHasher<object>();
            var compareResult = pwHasher.VerifyHashedPassword(null, hash, password + salt);
            return (compareResult == PasswordVerificationResult.Success);
        }

    }
}
