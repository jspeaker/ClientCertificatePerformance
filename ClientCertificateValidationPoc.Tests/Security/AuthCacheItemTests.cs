using System.Security.Cryptography.X509Certificates;
using ClientCertificateValidationPoc.Security;
using ClientCertificateValidationPoc.Tests.Certificates;
using ClientCertificateValidationPoc.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificateValidationPoc.Tests.Security
{
    [TestClass]
    public class AuthCacheItemTests
    {
        [TestMethod, TestCategory("Unit")]
        public void GivenEmptyCertificate_WhenRetrieveingPublicKey_ThenItShouldReturnEmptyString()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new X509Certificate2(), true);

            string actual = authCacheItem.PublicKey();

            actual.Should().BeEmpty();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenTestCertificate_WhenRetrievingPublicKey_ThenItShouldReturnCorrectKey()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new TestCertificate(new CertificateFile().Name("valid")).FromFile(), true);

            string publicKey = authCacheItem.PublicKey();

            publicKey.Should().StartWith("MIIBC");
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenCertificateValidity_WhenRetrieveingValidity_ThenItShouldReturnCorrectValue()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new X509Certificate2(), true);

            bool valid = authCacheItem.Valid();

            valid.Should().BeTrue();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenCertificateInvalidity_WhenRetrieveingValidity_ThenItShouldReturnCorrectValue()
        {
            IAuthCacheItem authCacheItem = new AuthCacheItem(new X509Certificate2(), false);

            bool valid = authCacheItem.Valid();

            valid.Should().BeFalse();
        }
    }
}