using System;
using System.Runtime.Caching;
using System.Security.Cryptography.X509Certificates;

namespace ClientCertificatePerformancePoc.Security
{
    public interface IChainPolicyCop
    {
        bool Legal(X509Certificate2 certificate);
    }

    public class ChainPolicyCop : IChainPolicyCop
    {
        public bool Legal(X509Certificate2 certificate)
        {
            if (!certificate.Populated()) return false;

            IAuthCacheItem cachedAuthItem = CachedAuthItem(certificate);
            if (cachedAuthItem.Populated()) return cachedAuthItem.Valid();

            LegalChain().Build(certificate);

            bool certificateIsValid = certificate.Verify();

            MemoryCache.Default.AddOrGetExisting(certificate.Thumbprint ?? string.Empty,
                new AuthCacheItem(certificate, certificateIsValid),
                new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(12)
                });
            return certificateIsValid;
        }

        private X509Chain LegalChain()
        {
            return new X509Chain
            {
                ChainPolicy =
                {
                    RevocationFlag = X509RevocationFlag.EntireChain,
                    RevocationMode = X509RevocationMode.Online,
                    VerificationFlags = X509VerificationFlags.AllFlags,
                    VerificationTime = DateTime.Now
                }
            };
        }

        private IAuthCacheItem CachedAuthItem(X509Certificate2 certificate)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            CacheItem cacheItem = MemoryCache.Default.GetCacheItem(certificate.Thumbprint);
            if (!ValidCacheItem(cacheItem, certificate)) return new NullAuthCacheItem();

            return (IAuthCacheItem)cacheItem?.Value;
        }

        private bool ValidCacheItem(CacheItem cacheItem, X509Certificate2 certificate)
        {
            return cacheItem != null && ((IAuthCacheItem) cacheItem.Value).Equals(certificate);
        }
    }
}