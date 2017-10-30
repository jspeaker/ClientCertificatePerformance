using System;
using System.Runtime.Caching;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http.Controllers;
using ClientCertificateValidationPoc.Security;
using ClientCertificateValidationPoc.Tests.Certificates;
using ClientCertificateValidationPoc.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificateValidationPoc.Tests.Security
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

            bool authorized = certificateAuthorizationAttribute.Authorized(_actionContextBuilder.ActionContext(true, true, new CertificateFile().Name("valid")));

            authorized.Should().BeTrue();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenSelfSignedCertificate_WhenCheckingAuthorization_ShouldReturnFalse()
        {
            AccessibleCertificateAuthorizationAttribute certificateAuthorizationAttribute = new AccessibleCertificateAuthorizationAttribute();

            bool authorized = certificateAuthorizationAttribute.Authorized(_actionContextBuilder.ActionContext(true, true, new CertificateFile().Name("invalid")));

            authorized.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenNullCertificateObject_WhenCheckingAuthorization_ShouldThrow()
        {
            AccessibleCertificateAuthorizationAttribute certificateAuthorizationAttribute = new AccessibleCertificateAuthorizationAttribute();

            Action action = () =>
            {
                certificateAuthorizationAttribute.Authorized(_actionContextBuilder.ActionContext(true, true, new CertificateFile().Name("null")));
            };

            action.ShouldThrow<Exception>();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenChainPolicyCopException_WhenCheckingAuthorization_ItShouldReturnFalse()
        {
            IChainPolicyCop chainPolicyCop = new FakeChainPolicyCopy();
            AccessibleCertificateAuthorizationAttribute certificateAuthorizationAttribute = new AccessibleCertificateAuthorizationAttribute(chainPolicyCop);

            bool authorized = certificateAuthorizationAttribute.Authorized(_actionContextBuilder.ActionContext(true, true, new CertificateFile().Name("valid")));

            authorized.Should().BeFalse();
        }
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