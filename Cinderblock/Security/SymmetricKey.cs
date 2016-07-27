using System;
using System.Security.Cryptography;

namespace Cinderblock.Security
{
    public sealed class SymmetricKey : IDisposable, IEquatable<SymmetricKey>
    {
        internal SymmetricKey() { }

        public byte[] Key { get; set; }
        public string CertificateHash { get; set; }

        internal static SymmetricKey Create(string thumbprint)
        {
            if (string.IsNullOrEmpty(thumbprint))
            {
                throw new ArgumentNullException("thumbprint");
            }

            var key = new SymmetricKey();

            using (var provider = new AesCryptoServiceProvider())
            {
                provider.GenerateKey();
                key.Key = provider.Key;
            }

            key.CertificateHash = thumbprint;

            return key;
        }

        #region IEquatable<SymmetricKey>

        /// <summary>
        ///     <para>Determines if two <see cref="SymmetricKey"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="SymmetricKey"/> object to compare.</param>
        /// <param name="y">The second <see cref="SymmetricKey"/> object to compare.</param>
        /// <returns>true if the first <see cref="SymmetricKey"/> is equal to the second <see cref="SymmetricKey"/>; otherwise false.</returns>
        /// <filterpriority>2</filterpriority>
        public static bool Equals(SymmetricKey x, SymmetricKey y)
        {
            if ((object)x == (object)y)
            {
                return true;
            }
            if ((object)x == null || (object)y == null)
            {
                return false;
            }
            return x.Key.ByteArrayCompare(y.Key) && x.CertificateHash.Equals(y.CertificateHash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     <para>Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.</para>
        /// </summary>
        /// <param name="objectToCompare">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise false.</returns>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object objectToCompare)
        {
            return SymmetricKey.Equals(this, objectToCompare as SymmetricKey);
        }

        /// <summary>
        ///     <para>Determines whether the specified <see cref="SymmetricKey"/> is equal to the current <see cref="SymmetricKey"/>.</para>
        /// </summary>
        /// <param name="other">The <see cref="SymmetricKey"/> to compare with the current <see cref="SymmetricKey"/>.</param>
        /// <returns>true if the specified <see cref="SymmetricKey"/> is equal to the current <see cref="SymmetricKey"/>; otherwise false.</returns>
        public bool Equals(SymmetricKey other)
        {
            return SymmetricKey.Equals(this, other);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="SymmetricKey"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="SymmetricKey"/> object to compare.</param>
        /// <param name="y">The second <see cref="SymmetricKey"/> object to compare.</param>
        /// <returns>true if the first <see cref="SymmetricKey"/> is equal to the second <see cref="SymmetricKey"/>; otherwise false.</returns>
        public static bool operator ==(SymmetricKey x, SymmetricKey y)
        {
            return SymmetricKey.Equals(x, y);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="SymmetricKey"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="SymmetricKey"/> object to compare.</param>
        /// <param name="y">The second <see cref="SymmetricKey"/> object to compare.</param>
        /// <returns>false if the first <see cref="SymmetricKey"/> is equal to the second <see cref="SymmetricKey"/>; otherwise true.</returns>
        public static bool operator !=(SymmetricKey x, SymmetricKey y)
        {
            return !SymmetricKey.Equals(x, y);
        }

        #endregion

        #region GetHashCode()

        /// <summary>
        ///     <para>Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table.</para>
        /// </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.Key.GetHashCode() ^ this.CertificateHash.GetHashCode();
        }

        #endregion

        #region IDisposable Members

        private bool disposed = false;

        /// <summary>
        ///     <para>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</para>
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     <para>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</para>
        /// </summary>
        /// <param name="disposing">Indicates if the method should dispose unmanaged resources.</param>
        /// <filterpriority>2</filterpriority>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.Key != null)
                    {
                        Array.Clear(this.Key, 0, this.Key.Length);
                    }
                }
            }
            this.disposed = true;
        }

        /// <summary>
        ///     <para>Performs Finalization tasks for the <see cref="SymmetricKey"/> object.</para>
        /// </summary>
        ~SymmetricKey()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
