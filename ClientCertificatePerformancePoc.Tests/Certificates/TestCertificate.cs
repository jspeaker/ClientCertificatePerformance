using System.Security.Cryptography.X509Certificates;
using ClientCertificatePerformancePoc.Tests.TestUtilities;

namespace ClientCertificatePerformancePoc.Tests.Certificates
{
    public class TestCertificate
    {
        private readonly IThisAssembly _thisAssembly;
        private readonly string _filename;

        public TestCertificate(string filename) : this(new ThisAssembly(), filename)  { }

        public TestCertificate(IThisAssembly thisAssembly, string filename)
        {
            _thisAssembly = thisAssembly;
            _filename = filename;
        }

        public X509Certificate2 FromFile()
        {
            if (string.IsNullOrEmpty(_filename)) return new X509Certificate2();

            string assemblyPath = _thisAssembly.Path();
            return new X509Certificate2(X509Certificate.CreateFromCertFile($"{assemblyPath}\\certificates\\{_filename}"));
        }
    }
}