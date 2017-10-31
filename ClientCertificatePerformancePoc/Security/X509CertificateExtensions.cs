using System.Security.Cryptography.X509Certificates;

namespace ClientCertificatePerformancePoc.Security
{
    public static class X509CertificateExtensions
    {
        public static bool Populated(this X509Certificate certificate)
        {
            return certificate.Handle.ToInt64() != 0 && certificate.GetRawCertData().Length > 0;
        }
    }
}