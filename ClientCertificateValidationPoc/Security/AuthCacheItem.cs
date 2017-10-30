using System;
using System.Security.Cryptography.X509Certificates;

namespace ClientCertificateValidationPoc.Security
{
    public class AuthCacheItem : IAuthCacheItem
    {
        private readonly X509Certificate2 _certificate;
        private readonly bool _valid;

        public AuthCacheItem(X509Certificate2 certificate, bool valid)
        {
            _certificate = certificate;
            _valid = valid;
        }

        public virtual string PublicKey()
        {
            if (_certificate.Handle.ToInt64().Equals(0)) return string.Empty;
            return Convert.ToBase64String(_certificate.PublicKey.EncodedKeyValue.RawData);
        }

        public virtual bool Valid()
        {
            return _valid;
        }
    }
}