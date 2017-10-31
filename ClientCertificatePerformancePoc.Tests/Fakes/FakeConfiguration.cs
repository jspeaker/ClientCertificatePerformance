using ClientCertificatePerformancePoc.Configuration;

namespace ClientCertificatePerformancePoc.Tests.Fakes
{
    public class FakeConfiguration : IConfiguration
    {
        public string ApimRequestVerification()
        {
            return "apimRequestVerification";
        }
    }
}