using System;
using System.Linq;
using System.Security.Cryptography;

namespace Cinderblock.Security
{
    public sealed class FauxCryptoProvider : ISymmetricCryptoProvider, IDisposable
    {
        #region ICryptoProvider Members

        public SecuredData Encrypt(IPayload payload)
        {
            return new SecuredData(FauxCryptoProvider.Shift(payload.Data), new byte[] { 1 });
        }

        public IPayload Decrypt(SecuredData data)
        {
            return new Payload(FauxCryptoProvider.Shift(data.Data));
        }

        private static byte[] Shift(byte[] bytes)
        {
            return bytes.Reverse().ToArray();
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

        public void Dispose()
        {
        }

        #endregion
    }
}
