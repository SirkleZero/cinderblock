using System;
using System.Security.Cryptography;

namespace Cinderblock.Security
{
    /// <summary>
    /// Provides an object container for a password that has been hashed with a specified nonce value.
    /// </summary>
    public sealed class SecuredPassword
    {
        /// <summary>
        /// Creates an instance of a <see cref="SecuredPassword"/> object that represents a password that has been secured by a Hash.
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="nonce">The nonce that was used to hash the password.</param>
        private SecuredPassword(string hashedPassword, byte[] nonce)
        {
            if (string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentNullException("hashedPassword");
            }
            if(nonce == null || nonce.Length.Equals(0))
            {
                throw new ArgumentOutOfRangeException("nonce", "The nonce value cannot be null or have a length of zero bytes.");
            }

            this.HashedPassword = hashedPassword;
            this.Nonce = nonce;
        }

        /// <summary>
        /// The hashed password.
        /// </summary>
        public string HashedPassword { get; set; }

        /// <summary>
        /// The nonce that was used to hash the password.
        /// </summary>
        public byte[] Nonce { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="SecuredPassword"/> object by hashing a clear text password with a cryptographically 
        /// random nonce size between 48 and 64 bytes in length.
        /// </summary>
        /// <param name="cleartextPassword">The password to hash.</param>
        /// <returns>A <see cref="SecuredPassword"/> containing the hashed password and it's corresponding nonce.</returns>
        public static SecuredPassword Create(string cleartextPassword)
        {
            return SecuredPassword.Create(cleartextPassword, 48, 64);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SecuredPassword"/> object by hashing a clear text password with a cryptographically 
        /// random nonce size between 48 and 64 bytes in length.
        /// </summary>
        /// <param name="cleartextPassword">The password to hash.</param>
        /// <param name="minimumSaltBytes">The minimum length in bytes for the nonce value.</param>
        /// <param name="maximumSaltBytes">The maximum length in bytes for the nonce value.</param>
        /// <returns>A <see cref="SecuredPassword"/> containing the hashed password and it's corresponding nonce.</returns>
        public static SecuredPassword Create(string cleartextPassword, int minimumSaltBytes, int maximumSaltBytes)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var salt = provider.GenerateSalt(minimumSaltBytes, maximumSaltBytes);
                return new SecuredPassword(cleartextPassword.Hash(HashType.SHA512, salt), salt);
            }
        }
    }
}
