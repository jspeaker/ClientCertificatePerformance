using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Controllers;
using ClientCertificateValidationPoc.Controllers;
using ClientCertificateValidationPoc.Tests.Certificates;

namespace ClientCertificateValidationPoc.Tests.TestUtilities
{
    public class ActionContextBuilder
    {
        public HttpActionContext ActionContext(bool includeHeader, bool populateHeader, string certificateName)
        {
            HttpRequestContext requestContext = new HttpRequestContext();
            HttpRequestMessage requestMessage = new HttpRequestMessage();

            if (includeHeader && !populateHeader) SetEmptyClientCertificateHeader(requestMessage);
            if (includeHeader && populateHeader) SetClientCertificateHeader(requestMessage, certificateName);

            HttpControllerDescriptor controllerDescriptor = new HttpControllerDescriptor(new HttpConfiguration(), "Values", typeof(ValuesController));
            HttpControllerContext controllerContext = new HttpControllerContext(requestContext, requestMessage, controllerDescriptor, new ValuesController());
            HttpActionContext actionContext = new HttpActionContext(controllerContext, new ReflectedHttpActionDescriptor());
            return actionContext;
        }

        private void SetEmptyClientCertificateHeader(HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.TryAddWithoutValidation("X-ARR-ClientCert", string.Empty);
        }

        private void SetClientCertificateHeader(HttpRequestMessage requestMessage, string certificateFileName)
        {
            byte[] exportedCertificate = new TestCertificate(certificateFileName).FromFile().Export(X509ContentType.Cert);
            requestMessage.Headers.TryAddWithoutValidation("X-ARR-ClientCert", Convert.ToBase64String(exportedCertificate));
        }
    }
}