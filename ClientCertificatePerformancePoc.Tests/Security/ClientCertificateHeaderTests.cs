using System.Security.Cryptography.X509Certificates;
using System.Web.Http.Controllers;
using ClientCertificatePerformancePoc.Security;
using ClientCertificatePerformancePoc.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Security
{
    [TestClass]
    public class ClientCertificateHeaderTests
    {
        private ActionContextBuilder _actionContextBuilder;

        [TestInitialize]
        public void Setup()
        {
            _actionContextBuilder = new ActionContextBuilder();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenNoClientCertificatesHeader_WhenRetrievingClientCertificate_ShouldReturnNullObject()
        {
            IClientCertificateHeader clientCertificateHeader = new ClientCertificateHeader();
            X509Certificate2 clientCertificate = clientCertificateHeader.ClientCertificate(_actionContextBuilder.ActionContext());

            clientCertificate.Handle.ToInt64().Should().Be(0);
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenEmptyClientCertificatesHeader_WhenRetrievingClientCertificate_ShouldReturnNullObject()
        {
            IClientCertificateHeader clientCertificateHeader = new ClientCertificateHeader();
            X509Certificate2 clientCertificate = clientCertificateHeader.ClientCertificate(_actionContextBuilder.ActionContext());

            clientCertificate.Handle.ToInt64().Should().Be(0);
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenClientCertificatesHeader_WhenRetrievingClientCertificate_ShouldNotReturnNullObject()
        {
            IClientCertificateHeader clientCertificateHeader = new ClientCertificateHeader();
            HttpActionContext actionContext = _actionContextBuilder.ActionContext().WithClientCertificateHeader(new CertificateFile().Name("valid"));
            X509Certificate2 clientCertificate = clientCertificateHeader.ClientCertificate(actionContext);

            clientCertificate.Handle.ToInt64().Should().NotBe(0);
        }
    }
}