using System.Collections.Generic;

namespace ClientCertificatePerformancePoc.Logging
{
    public interface ILogDestination
    {
        void Trace(string message);
        IEnumerable<string> Print();
    }
}