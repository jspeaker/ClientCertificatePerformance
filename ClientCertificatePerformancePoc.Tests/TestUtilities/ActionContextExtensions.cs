using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http.Controllers;
using ClientCertificatePerformancePoc.Tests.Certificates;

namespace ClientCertificatePerformancePoc.Tests.TestUtilities
{
    public static class ActionContextExtensions
    {
        public static HttpActionContext WithApimRequestVerificationHeader(this HttpActionContext actionContext, string headerValue)
        {
            actionContext.Request.Headers.TryAddWithoutValidation("apim-request-verification", headerValue);
            return actionContext;
        }

        public static HttpActionContext WithClientCertificateHeader(this HttpActionContext actionContext, string certificateFileName)
        {
            actionContext.Request.Headers.TryAddWithoutValidation("X-ARR-ClientCert", Certificate(certificateFileName));
            return actionContext;
        }

        private static string Certificate(string certificateFileName)
        {
            if (string.IsNullOrWhiteSpace(certificateFileName)) return string.Empty;

            byte[] exportedCertificate = new TestCertificate(certificateFileName).FromFile().Export(X509ContentType.Cert);
            return Convert.ToBase64String(exportedCertificate);
        }
    }
}