using System.Security.Cryptography.X509Certificates;
using ClientCertificatePerformancePoc.Security;
using ClientCertificatePerformancePoc.Tests.Certificates;
using ClientCertificatePerformancePoc.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Security
{
    [TestClass]
    public class AuthCacheItemTests
    {
        [TestMethod, TestCategory("Unit")]
        public void GivenEmptyCertificate_WhenCheckingIfPopulated_ThenItShouldReturnFalse()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new X509Certificate2(), true);

            bool actual = authCacheItem.Populated();

            actual.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenTestCertificate_WhenCheckingIfPopulated_ThenItShouldReturnTrue()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new TestCertificate(new CertificateFile().Name("valid")).FromFile(), true);

            bool actual = authCacheItem.Populated();

            actual.Should().BeTrue();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenCertificateValidity_WhenRetrievingValidity_ThenItShouldReturnCorrectValue()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new X509Certificate2(), true);

            bool valid = authCacheItem.Valid();

            valid.Should().BeTrue();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenCertificateInvalidity_WhenRetrievingValidity_ThenItShouldReturnCorrectValue()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new X509Certificate2(), false);

            bool valid = authCacheItem.Valid();

            valid.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenDuplicativeComparisonCertificate_WhenCallingEquals_ThenItShouldReturnTrue()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new TestCertificate(new CertificateFile().Name("valid")).FromFile(), false);
            X509Certificate2 x509Certificate2 = new TestCertificate(new CertificateFile().Name("valid")).FromFile();

            bool actual = authCacheItem.Equals(x509Certificate2);

            actual.Should().BeTrue();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenUncastableComparatorCertificate_WhenCallingEquals_ThenItShouldReturnFalse()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new TestCertificate(new CertificateFile().Name("null")).FromFile(), false);
            object obj = new object();

            bool actual = authCacheItem.Equals(obj);

            actual.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenUnpopulatedComparisonCertificate_WhenCallingEquals_ThenItShouldReturnFalse()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new TestCertificate(new CertificateFile().Name("null")).FromFile(), false);
            X509Certificate2 x509Certificate2 = new TestCertificate(new CertificateFile().Name("valid")).FromFile();

            bool actual = authCacheItem.Equals(x509Certificate2);

            actual.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenUnpopulatedComparatorCertificate_WhenCallingEquals_ThenItShouldReturnFalse()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new TestCertificate(new CertificateFile().Name("valid")).FromFile(), true);
            X509Certificate2 x509Certificate2 = new TestCertificate(new CertificateFile().Name("null")).FromFile();

            bool actual = authCacheItem.Equals(x509Certificate2);

            actual.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenUnpopulatedComparatorAndComparisonCertificates_WhenCallingEquals_ThenItShouldReturnTrue()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new TestCertificate(new CertificateFile().Name("null")).FromFile(), true);
            X509Certificate2 x509Certificate2 = new TestCertificate(new CertificateFile().Name("null")).FromFile();

            bool actual = authCacheItem.Equals(x509Certificate2);

            actual.Should().BeTrue();
        }
    }
}