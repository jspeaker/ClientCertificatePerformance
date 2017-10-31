using System;
using System.Runtime.Caching;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http.Controllers;
using ClientCertificatePerformancePoc.Security;
using ClientCertificatePerformancePoc.Tests.Certificates;
using ClientCertificatePerformancePoc.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Security
{
    [TestClass]
    public class CertificateAuthorizationAttributeTests
    {
        private ActionContextBuilder _actionContextBuilder;

        [TestInitialize]
        public void Setup()
        {
            _actionContextBuilder = new ActionContextBuilder();
        }

        [TestCleanup]
        public void Teardown()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            MemoryCache.Default.Remove(new TestCertificate("jim.speaker.cer").FromFile().Thumbprint);
            MemoryCache.Default.Remove(new TestCertificate("iisexpress-self-signed.cer").FromFile().Thumbprint);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenValidClientCertificate_WhenCheckingAuthorization_ShouldReturnTrue()
        {
            AccessibleCertificateAuthorizationAttribute certificateAuthorizationAttribute = new AccessibleCertificateAuthorizationAttribute();
            HttpActionContext actionContext = _actionContextBuilder
                .ActionContext()
                .WithClientCertificateHeader(new CertificateFile().Name("valid"));

            bool authorized = certificateAuthorizationAttribute.Authorized(actionContext);

            authorized.Should().BeTrue();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenSelfSignedCertificate_WhenCheckingAuthorization_ShouldReturnFalse()
        {
            AccessibleCertificateAuthorizationAttribute certificateAuthorizationAttribute = new AccessibleCertificateAuthorizationAttribute();
            HttpActionContext actionContext = _actionContextBuilder
                .ActionContext()
                .WithClientCertificateHeader(new CertificateFile().Name("invalid"));

            bool authorized = certificateAuthorizationAttribute.Authorized(actionContext);

            authorized.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenNullCertificateObject_WhenCheckingAuthorization_ItShouldReturnFalse()
        {
            AccessibleCertificateAuthorizationAttribute certificateAuthorizationAttribute = new AccessibleCertificateAuthorizationAttribute();
            HttpActionContext actionContext = _actionContextBuilder
                .ActionContext()
                .WithClientCertificateHeader(new CertificateFile().Name("null"));

            bool authorized = certificateAuthorizationAttribute.Authorized(actionContext);

            authorized.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenChainPolicyCopException_WhenCheckingAuthorization_ItShouldThrowException()
        {
            IChainPolicyCop chainPolicyCop = new FakeChainPolicyCopy();
            AccessibleCertificateAuthorizationAttribute certificateAuthorizationAttribute = new AccessibleCertificateAuthorizationAttribute(chainPolicyCop);
            HttpActionContext actionContext = _actionContextBuilder
                .ActionContext()
                .WithClientCertificateHeader(new CertificateFile().Name("valid"));

            Action action = () =>
            {
                certificateAuthorizationAttribute.Authorized(actionContext);
            };

            action.ShouldThrow<Exception>();        }
    }

    public class AccessibleCertificateAuthorizationAttribute : CertificateAuthorizationAttribute
    {
        public AccessibleCertificateAuthorizationAttribute() { }
        public AccessibleCertificateAuthorizationAttribute(IChainPolicyCop chainPolicyCop) : base(chainPolicyCop) { }

        public bool Authorized(HttpActionContext actionContext)
        {
            return IsAuthorized(actionContext);
        }
    }

    public class FakeChainPolicyCopy : IChainPolicyCop
    {
        public bool Legal(X509Certificate2 certificate)
        {
            throw new NotImplementedException();
        }
    }
}