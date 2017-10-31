using System;
using System.Security.Cryptography.X509Certificates;

namespace ClientCertificatePerformancePoc.Security
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class AuthCacheItem : IAuthCacheItem
    {
        private readonly X509Certificate2 _certificate;
        private readonly bool _valid;

        public AuthCacheItem(X509Certificate2 certificate, bool valid)
        {
            _certificate = certificate;
            _valid = valid;
        }

        public bool Populated() => _certificate.Populated();

        public bool Valid() => _valid;

        public override bool Equals(object obj)
        {
            // ReSharper disable once UsePatternMatching
            X509Certificate2 certificate = obj as X509Certificate2;
            if (certificate == null) return false;

            if (certificate.Populated() && !Populated()) return false;

            if (!certificate.Populated() && Populated()) return false;

            if (!certificate.Populated() && !Populated()) return true;

            return Convert.ToBase64String(certificate.RawData) == Convert.ToBase64String(_certificate.RawData);
        }
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}