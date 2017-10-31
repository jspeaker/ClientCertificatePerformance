namespace ClientCertificatePerformancePoc.Tests.TestUtilities
{
    public class CertificateFile
    {
        public string Name(string certificateType)
        {
            if (certificateType == new TestCertificateType().Valid()) return "jim.speaker.cer";
            if (certificateType == new TestCertificateType().Invalid()) return "iisexpress-self-signed.cer";
            return string.Empty;
        }

        private class TestCertificateType
        {
            public string Valid() => "valid";
            public string Invalid() => "invalid";
        }
    }
}