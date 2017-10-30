using System;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Security.Cryptography.X509Certificates;
using Microsoft.ApplicationInsights;

namespace ClientCertificateValidationPoc.Security
{
    public interface IChainPolicyCop
    {
        bool Legal(X509Certificate2 certificate);
    }

    public class ChainPolicyCop : IChainPolicyCop
    {
        private readonly TelemetryClient _telemetryClient;

        public ChainPolicyCop() : this(new TelemetryClient()) { }

        public ChainPolicyCop(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public bool Legal(X509Certificate2 certificate)
        {
            if (!PopulatedCertificate(certificate)) return false;

            IAuthCacheItem cachedAuthItem = CachedAuthItem(certificate);
            if (!string.IsNullOrEmpty(cachedAuthItem.PublicKey())) return cachedAuthItem.Valid();

            X509Chain x509Chain = LegalChain();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            x509Chain.Build(certificate);
            stopwatch.Stop();
            _telemetryClient.TrackTrace($"It took {stopwatch.ElapsedMilliseconds} to build chain policy.");

            stopwatch.Reset();
            stopwatch.Start();
            bool certificateIsValid = certificate.Verify();
            HandleChainStatusErrors(x509Chain);
            stopwatch.Stop();
            _telemetryClient.TrackTrace($"It took {stopwatch.ElapsedMilliseconds} to verify certificate based on chain policy.");

            MemoryCache.Default.AddOrGetExisting(certificate.Thumbprint ?? string.Empty, new AuthCacheItem(certificate, certificateIsValid), new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60)
            });
            return certificateIsValid;
        }

        private void HandleChainStatusErrors(X509Chain x509Chain)
        {
            foreach (X509ChainStatus status in x509Chain.ChainStatus)
            {
                _telemetryClient.TrackTrace($"LegalChain status: {status.StatusInformation}");
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

            return (IAuthCacheItem) cacheItem?.Value;
        }

        private bool PopulatedCertificate(X509Certificate certificate)
        {
            if (certificate.Handle.ToInt64().Equals(0)) return false;
            return true;
        }

        private bool ValidCacheItem(CacheItem cacheItem, X509Certificate2 certificate)
        {
            return cacheItem != null && 
                ((IAuthCacheItem) cacheItem.Value).PublicKey() == Convert.ToBase64String(certificate.PublicKey.EncodedKeyValue.RawData);
        }
    }
}