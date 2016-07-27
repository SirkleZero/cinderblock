using System;

namespace Cinderblock.Security
{
    /// <summary>
    /// Provides an interface that defines the functionality of a symmetric key repository; a data store for symmetric keys.
    /// </summary>
    public interface ISymmetricKeyRepository
    {
        /// <summary>
        /// Saves the specified key to the data store.
        /// </summary>
        /// <param name="key">The <see cref="SymmetricKey"/> to store.</param>
        void Save(SymmetricKey key);

        /// <summary>
        /// Gets the <see cref="SymmetricKey"/> from the data store.
        /// </summary>
        /// <returns>A <see cref="SymmetricKey"/>.</returns>
        SymmetricKey GetKey();
    }
}
