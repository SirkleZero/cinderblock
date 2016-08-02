using System;
using System.Security.Cryptography;

namespace Cinderblock.Security
{
    /// <summary>
    /// Provides an object container for a password that has been hashed with a specified salt value.
    /// </summary>
    public sealed class SecuredPassword
    {
        /// <summary>
        /// Specifies the default minimum size in bytes for the salt.
        /// </summary>
        private const int defaultMinimumSaltSize = 48;

        /// <summary>
        /// Specifies the default maximum size in bytes for the salt.
        /// </summary>
        private const int defaultMaximumSaltSize = 64;

        /// <summary>
        /// Creates an instance of a <see cref="SecuredPassword"/> object that represents a password that has been secured by a Hash.
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="salt">The salt that was used to hash the password.</param>
        private SecuredPassword(string hashedPassword, byte[] salt)
        {
            this.HashedPassword = hashedPassword;
            this.Salt = salt;
        }

        /// <summary>
        /// The hashed password.
        /// </summary>
        public string HashedPassword { get; private set; }

        /// <summary>
        /// The salt that was used to hash the password.
        /// </summary>
        public byte[] Salt { get; private set; }

        /// <summary>
        /// Creates an instance of the <see cref="SecuredPassword"/> object by hashing a clear text password with a new cryptographically 
        /// random salt size between 48 and 64 bytes in length.
        /// </summary>
        /// <param name="cleartextPassword">The password to hash.</param>
        /// <returns>A <see cref="SecuredPassword"/> containing the hashed password and it's corresponding salt.</returns>
        public static SecuredPassword Create(string cleartextPassword)
        {
            return SecuredPassword.Create(cleartextPassword, SecuredPassword.defaultMinimumSaltSize, SecuredPassword.defaultMaximumSaltSize);
        }

        /// <summary>
        /// Creates an instance of the <see cref="SecuredPassword"/> object by hashing a clear text password with a new cryptographically 
        /// random salt size between the minimum and maximum byte sizes.
        /// </summary>
        /// <param name="cleartextPassword">The password to hash.</param>
        /// <param name="minimumSaltBytes">The minimum length in bytes for the salt value.</param>
        /// <param name="maximumSaltBytes">The maximum length in bytes for the salt value.</param>
        /// <returns>A <see cref="SecuredPassword"/> containing the hashed password and it's corresponding salt.</returns>
        public static SecuredPassword Create(string cleartextPassword, int minimumSaltBytes, int maximumSaltBytes)
        {
            if (string.IsNullOrEmpty(cleartextPassword))
            {
                throw new ArgumentNullException("cleartextPassword");
            }
            if (minimumSaltBytes.Equals(0) || minimumSaltBytes > maximumSaltBytes)
            {
                throw new ArgumentOutOfRangeException("minimumSaltSize", "The minimum salt size cannot be zero, or be greater than the maximum size.");
            }
            if (maximumSaltBytes.Equals(0))
            {
                throw new ArgumentOutOfRangeException("maximumSaltSize", "The maximum salt size cannot be zero.");
            }

            using (var provider = new RNGCryptoServiceProvider())
            {
                var salt = provider.GenerateSalt(minimumSaltBytes, maximumSaltBytes);
                return SecuredPassword.Create(cleartextPassword, salt);
            }
        }

        /// <summary>
        /// Creates an instance of the <see cref="SecuredPassword"/> object by hashing a clear text password with a predefined salt.
        /// </summary>
        /// <param name="cleartextPassword">The password to hash.</param>
        /// <param name="salt">The salt value to use when hashing the password.</param>
        /// <returns>A <see cref="SecuredPassword"/> containing the hashed password and it's corresponding salt.</returns>
        public static SecuredPassword Create(string cleartextPassword, byte[] salt)
        {
            if (string.IsNullOrEmpty(cleartextPassword))
            {
                throw new ArgumentNullException("cleartextPassword");
            }
            if (salt == null || salt.Length.Equals(0))
            {
                throw new ArgumentOutOfRangeException("salt", "The salt value cannot be null or have an array length equal to zero.");
            }

            return new SecuredPassword(cleartextPassword.Hash(HashType.SHA512, salt), salt);
        }
    }
}
