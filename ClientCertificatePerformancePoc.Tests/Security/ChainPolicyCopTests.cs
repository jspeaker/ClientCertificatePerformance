using System;
using System.Runtime.Caching;
using System.Security.Cryptography.X509Certificates;
using ClientCertificatePerformancePoc.Security;
using ClientCertificatePerformancePoc.Tests.Certificates;
using ClientCertificatePerformancePoc.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Security
{
    [TestClass]
    public class ChainPolicyCopTests
    {
        [TestMethod, TestCategory("Unit")]
        public void GivenCachedResult_WhenCheckingLegality_ItShouldReturnCachedResult()
        {
            X509Certificate2 certificate = new TestCertificate(new CertificateFile().Name("valid")).FromFile();
            MemoryCache.Default.AddOrGetExisting(certificate.Thumbprint ?? string.Empty, new AuthCacheItem(certificate, false), new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
            });
            ChainPolicyCop chainPolicyCop = new ChainPolicyCop();

            bool actual = chainPolicyCop.Legal(certificate);

            actual.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenNullCertificateObject_WhenCheckingLegality_ItShouldReturnFalse()
        {
            X509Certificate2 certificate = new TestCertificate(new CertificateFile().Name("null")).FromFile();
            ChainPolicyCop chainPolicyCop = new ChainPolicyCop();

            bool actual = chainPolicyCop.Legal(certificate);

            actual.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenNoCachedResult_WhenCheckingLegality_ItShouldReturnFreshResult()
        {
            X509Certificate2 certificate = new TestCertificate(new CertificateFile().Name("valid")).FromFile();
            MemoryCache.Default.Remove(certificate.Thumbprint ?? string.Empty);

            ChainPolicyCop chainPolicyCop = new ChainPolicyCop();

            bool actual = chainPolicyCop.Legal(certificate);

            actual.Should().BeTrue();
        }
    }
}