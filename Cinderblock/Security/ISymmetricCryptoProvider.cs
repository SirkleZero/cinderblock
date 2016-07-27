using System;

namespace Cinderblock.Security
{
    /// <summary>
    /// Provides an interface that describes the operations of symmetric key encryption.
    /// </summary>
    public interface ISymmetricCryptoProvider : IDisposable
    {
        /// <summary>
        /// Encrypts data.
        /// </summary>
        /// <param name="payload">The <see cref="IPayload"/> object that contains the data that needs to be encrypted.</param>
        /// <returns>A <see cref="SecuredData"/> object that contains an encrypted version of the <see cref="IPayload"/> data.</returns>
        SecuredData Encrypt(IPayload payload);

        /// <summary>
        /// Decrypts data.
        /// </summary>
        /// <param name="data">The <see cref="SecuredData"/> object that contains the encrypted data to be decrypted.</param>
        /// <returns>A <see cref="IPayload"/> object that contains the unecrypted version of the <see cref="SecuredData"/> data.</returns>
        IPayload Decrypt(SecuredData data);

        /// <summary>
        /// Generates an encryption key appropriate to the provider.
        /// </summary>
        /// <returns>A byte array that represents the value of the key.</returns>
        byte[] GenerateKey();

        /// <summary>
        /// Generates an encryption initialization vector appropriate to the provider.
        /// </summary>
        /// <returns>A byte array that represents the value of the initialization vector.</returns>
        byte[] GenerateIV();
    }
}
