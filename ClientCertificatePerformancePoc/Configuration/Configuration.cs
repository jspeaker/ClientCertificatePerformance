using System.Configuration;

namespace ClientCertificatePerformancePoc.Configuration
{
    public interface IConfiguration
    {
        string ApimRequestVerification();
    }

    public class Configuration : IConfiguration
    {
        public string ApimRequestVerification()
        {
            return ConfigurationManager.AppSettings["apim-request-verification"];
        }
    }
}