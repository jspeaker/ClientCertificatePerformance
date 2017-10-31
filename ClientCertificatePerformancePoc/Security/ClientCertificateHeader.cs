using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http.Controllers;

namespace ClientCertificatePerformancePoc.Security
{
    public interface IClientCertificateHeader
    {
        X509Certificate2 ClientCertificate(HttpActionContext actionContext);
    }

    public class ClientCertificateHeader : IClientCertificateHeader
    {
        public X509Certificate2 ClientCertificate(HttpActionContext actionContext)
        {
            actionContext.Request.Headers.TryGetValues("X-ARR-ClientCert", out IEnumerable<string> clientCerts);
            if (clientCerts == null) return new X509Certificate2();

            List<string> clientCertList = clientCerts.ToList();
            if (clientCertList.All(string.IsNullOrWhiteSpace)) return new X509Certificate2();

            return new X509Certificate2(Convert.FromBase64String(clientCertList.First()));
        }
    }
}