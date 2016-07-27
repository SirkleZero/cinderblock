using Cinderblock.Caching;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;

namespace Cinderblock.Security
{
    public sealed class VisualStudioSymmetricKeyRepository : ISymmetricKeyRepository
    {
        private const string CacheKey = "SymmetricKey_Key";

        private X509Certificate2 certificate = null;
        private string symmetricKeyFilePath;
        private readonly string thumbprint;
        private readonly ICache cache;
        private readonly TimeSpan cacheDuration;

        public VisualStudioSymmetricKeyRepository(string thumbprint, ICache cache, TimeSpan cacheDuration)
        {
            if (string.IsNullOrEmpty(thumbprint))
            {
                throw new ArgumentNullException("thumbprint");
            }
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            this.thumbprint = thumbprint;
            this.cache = cache;
            this.cacheDuration = cacheDuration;
            this.Initialize();
        }

        private void Initialize()
        {
            this.certificate = VisualStudioSymmetricKeyRepository.GetCertificate(this.thumbprint);
            if (this.certificate == null)
            {
                throw new ApplicationException("The certificate could not be found for the specified thumbprint.");
            }

            var webRootPath = HostingEnvironment.MapPath("/");
            this.symmetricKeyFilePath = Path.GetFullPath(Path.Combine(webRootPath, @"..\Encryption\", this.thumbprint));
        }

        public void Save(SymmetricKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            #region secure the key's key

            this.EncryptKey(key);

            #endregion

            #region store the encrypted key

            var serialized = VisualStudioSymmetricKeyRepository.Serialize(key);

            using (var file = File.CreateText(this.symmetricKeyFilePath))
            {
                file.Write(serialized);
            }

            #endregion

            // once saved, update the cache with the new encrypted key
            if (this.cache.Exists(VisualStudioSymmetricKeyRepository.CacheKey))
            {
                this.cache.Remove(VisualStudioSymmetricKeyRepository.CacheKey);
            }

            this.cache.Add(VisualStudioSymmetricKeyRepository.CacheKey, key, this.cacheDuration);
        }

        public SymmetricKey GetKey()
        {
            SymmetricKey key = null;
            if (!this.cache.TryGetValue<SymmetricKey>(VisualStudioSymmetricKeyRepository.CacheKey, out key))
            {
                if (File.Exists(this.symmetricKeyFilePath))
                {
                    using (var file = File.OpenText(this.symmetricKeyFilePath))
                    {
                        var serialized = file.ReadToEnd();
                        if (!string.IsNullOrEmpty(serialized))
                        {
                            key = VisualStudioSymmetricKeyRepository.Deserialize(serialized);
                            this.cache.Add(VisualStudioSymmetricKeyRepository.CacheKey, key, this.cacheDuration);
                        }
                    }
                }
                else
                {
                    // there is no key stored, create a new one and save it
                    key = SymmetricKey.Create(this.thumbprint);
                    this.Save(key);
                }
            }

            if (key != null)
            {
                var k = key.Copy();
                this.DecryptKey(k);
                return k;
            }

            return null;
        }

        private void EncryptKey(SymmetricKey key)
        {
            using (var rsa = this.certificate.PublicKey.Key as RSACryptoServiceProvider)
            {
                key.Key = rsa.Encrypt(key.Key, false);
            }
        }

        private void DecryptKey(SymmetricKey key)
        {
            using (var rsa = this.certificate.PrivateKey as RSACryptoServiceProvider)
            {
                key.Key = rsa.Decrypt(key.Key, false);
            }
        }

        private static X509Certificate2 GetCertificate(string thumbprint)
        {
            if (string.IsNullOrEmpty(thumbprint))
            {
                throw new ArgumentNullException("thumbprint");
            }

            var tp = thumbprint.Replace(" ", string.Empty);
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            X509Certificate2 certificate = null;

            try
            {
                store.Open(OpenFlags.ReadOnly);

                foreach (var cert in store.Certificates)
                {
                    if (!cert.HasPrivateKey)
                    {
                        continue;
                    }

                    if (cert.Thumbprint.Equals(tp, StringComparison.OrdinalIgnoreCase))
                    {
                        certificate = cert;
                        break;
                    }
                }
            }
            finally
            {
                store.Close();
            }

            return certificate;
        }

        private static string Serialize(SymmetricKey key)
        {
            return JsonConvert.SerializeObject(key);
        }

        private static SymmetricKey Deserialize(string data)
        {
            return JsonConvert.DeserializeObject<SymmetricKey>(data);
        }
    }
}
