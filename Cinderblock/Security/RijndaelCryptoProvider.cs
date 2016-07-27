using System;
using System.IO;
using System.Security.Cryptography;

namespace Cinderblock.Security
{
    public sealed class RijndaelCryptoProvider : ISymmetricCryptoProvider, IDisposable
    {
        private Rijndael algorithm;
        private readonly ISymmetricKeyRepository keyRepository;
        private readonly SymmetricKey key;

        public RijndaelCryptoProvider(ISymmetricKeyRepository keyRepository)
        {
            if (keyRepository == null)
            {
                throw new ArgumentNullException("keyRepository");
            }

            this.keyRepository = keyRepository;
            this.key = this.keyRepository.GetKey();

            if (this.key == null)
            {
                throw new ArgumentException("No key found.");
            }

            this.Initialize();
        }

        private void Initialize()
        {
            this.algorithm = Rijndael.Create();
            this.algorithm.KeySize = 256;
            this.algorithm.Mode = CipherMode.CBC;
            this.algorithm.Key = this.key.Key;
        }

        #region ICryptoProvider Members

        public SecuredData Encrypt(IPayload payload)
        {
            if (payload == null)
            {
                throw new ArgumentOutOfRangeException("payload");
            }
            if (payload.Data.Length.Equals(0))
            {
                throw new ArgumentOutOfRangeException("payload");
            }

            this.algorithm.GenerateIV();
            var encryptor = this.algorithm.CreateEncryptor(this.algorithm.Key, this.algorithm.IV);

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var binaryWriter = new BinaryWriter(cryptoStream))
                    {
                        binaryWriter.Write(payload.Data);
                    }
                }

                return new SecuredData(memoryStream.ToArray(), this.algorithm.IV);
            }
        }

        public IPayload Decrypt(SecuredData data)
        {
            if (data == null)
            {
                throw new ArgumentOutOfRangeException("data");
            }
            if (data.Data.Length.Equals(0))
            {
                throw new ArgumentOutOfRangeException("data");
            }

            Payload ret = null;
            
            this.algorithm.IV = data.IV;
            var decryptor = this.algorithm.CreateDecryptor(this.algorithm.Key, this.algorithm.IV);

            using (var memoryStream = new MemoryStream(data.Data))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (var binaryReader = new BinaryReader(cryptoStream))
                    {
                        var decrypted = binaryReader.ReadAllBytes();
                        ret = new Payload(decrypted);
                    }
                }
            }

            return ret;
        }

        public byte[] GenerateKey()
        {
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.GenerateKey();
                return provider.Key;
            }
        }

        public byte[] GenerateIV()
        {
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.GenerateIV();
                return provider.Key;
            }
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
                    if (this.key != null)
                    {
                        this.key.Dispose();
                    }
                    if (this.algorithm != null)
                    {
                        this.algorithm.Dispose();
                    }
                }
            }
            this.disposed = true;
        }

        /// <summary>
        ///     <para>Performs Finalization tasks for the <see cref="RijndaelCryptoProvider"/> object.</para>
        /// </summary>
        ~RijndaelCryptoProvider()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
