using System;
using System.Runtime.Caching;
using System.Security.Cryptography.X509Certificates;
using ClientCertificatePerformancePoc.Logging;

namespace ClientCertificatePerformancePoc.Security
{
    public interface IChainPolicyCop
    {
        bool Legal(X509Certificate2 certificate);
    }

    public class ChainPolicyCop : IChainPolicyCop
    {
        private readonly ILogDestination _logDestination;

        public ChainPolicyCop() : this(new TelemetryClientWrapper()) { }

        public ChainPolicyCop(ILogDestination logDestination)
        {
            _logDestination = logDestination;
        }

        public bool Legal(X509Certificate2 certificate)
        {
            if (!certificate.Populated()) return false;

            IAuthCacheItem cachedAuthItem = CachedAuthItem(certificate);
            if (cachedAuthItem.Populated()) return cachedAuthItem.Valid();

            X509Chain x509Chain = LegalChain();

            x509Chain.Build(certificate);

            bool certificateIsValid = certificate.Verify();
            HandleChainStatusErrors(x509Chain);

            MemoryCache.Default.AddOrGetExisting(certificate.Thumbprint ?? string.Empty, new AuthCacheItem(certificate, certificateIsValid), new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(12)
            });
            return certificateIsValid;
        }

        private void HandleChainStatusErrors(X509Chain x509Chain)
        {
            foreach (X509ChainStatus status in x509Chain.ChainStatus)
            {
                _logDestination.Trace($"LegalChain status: {status.StatusInformation}");
            }
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