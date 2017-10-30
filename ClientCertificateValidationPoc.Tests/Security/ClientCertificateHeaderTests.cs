using System.Security.Cryptography.X509Certificates;
using ClientCertificateValidationPoc.Security;
using ClientCertificateValidationPoc.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificateValidationPoc.Tests.Security
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
            X509Certificate2 clientCertificate = clientCertificateHeader.ClientCertificate(_actionContextBuilder.ActionContext(false, false, string.Empty));

            clientCertificate.Handle.ToInt64().Should().Be(0);
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenEmptyClientCertificatesHeader_WhenRetrievingClientCertificate_ShouldReturnNullObject()
        {
            IClientCertificateHeader clientCertificateHeader = new ClientCertificateHeader();
            X509Certificate2 clientCertificate = clientCertificateHeader.ClientCertificate(_actionContextBuilder.ActionContext(true, false, string.Empty));

            clientCertificate.Handle.ToInt64().Should().Be(0);
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenClientCertificatesHeader_WhenRetrievingClientCertificate_ShouldNotReturnNullObject()
        {
            IClientCertificateHeader clientCertificateHeader = new ClientCertificateHeader();
            X509Certificate2 clientCertificate = clientCertificateHeader.ClientCertificate(_actionContextBuilder.ActionContext(true, true, new CertificateFile().Name("valid")));

            clientCertificate.Handle.ToInt64().Should().NotBe(0);
        }
    }
}