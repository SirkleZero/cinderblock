using System;
using System.Text;

namespace Cinderblock.Security
{
    /// <summary>
    /// The <see cref="Payload"/> object contains unencrypted data.
    /// </summary>
    public class Payload : IPayload
    {
        #region constructors

        /// <summary>
        /// Creates a new <see cref="Payload"/> object.
        /// </summary>
        internal Payload() { }

        /// <summary>
        /// Creates a new <see cref="Payload"/> object.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Payload(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.Data = data;
        }

        #endregion

        public byte[] Data { get; internal set; }

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
                }
            }

            this.disposed = true;
        }

        /// <summary>
        ///     <para>Performs Finalization tasks for the <see cref="Payload"/> object.</para>
        /// </summary>
        ~Payload()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
