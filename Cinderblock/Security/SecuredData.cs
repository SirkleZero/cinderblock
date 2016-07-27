using System;

namespace Cinderblock.Security
{
    /// <summary>
    /// The <see cref="SecuredData"/> object contains encrypted data and an initialization vector used to store or decrypt information.
    /// </summary>
    public class SecuredData : IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="SecuredData"/> object.
        /// </summary>
        /// <param name="data">The encrypted data to decrypt. If specified with an initialization vector, then it is assumed that the data parameter is an encrypted value.</param>
        /// <param name="iv">The initialization vector used to encrypt the data.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SecuredData(byte[] data, byte[] iv)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Length.Equals(0))
            {
                throw new ArgumentOutOfRangeException("data", "The length of the data array must be greater than zero.");
            }
            if (iv == null)
            {
                throw new ArgumentNullException("iv");
            }
            if (iv.Length.Equals(0))
            {
                throw new ArgumentOutOfRangeException("data", "The length of the iv array must be greater than zero.");
            }

            this.Data = data;
            this.IV = iv;
        }

        public byte[] Data { get; set; }
        public byte[] IV { get; set; }

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
                    if (this.Data != null)
                    {
                        Array.Clear(this.Data, 0, this.Data.Length);
                    }
                    if (this.IV != null)
                    {
                        Array.Clear(this.IV, 0, this.IV.Length);
                    }
                }
            }
            this.disposed = true;
        }

        /// <summary>
        ///     <para>Performs Finalization tasks for the <see cref="SecuredData"/> object.</para>
        /// </summary>
        ~SecuredData()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
