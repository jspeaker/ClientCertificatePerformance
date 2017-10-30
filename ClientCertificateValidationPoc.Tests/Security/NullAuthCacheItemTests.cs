using ClientCertificateValidationPoc.Security;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificateValidationPoc.Tests.Security
{
    [TestClass]
    public class NullAuthCacheItemTests
    {
        [TestMethod, TestCategory("Unit")]
        public void PublicKeyShouldReturnEmptyString()
        {
            NullAuthCacheItem nullAuthCacheItem = new NullAuthCacheItem();
            nullAuthCacheItem.PublicKey().Should().BeEmpty();
        }

        [TestMethod, TestCategory("Unit")]
        public void ValidShouldReturnFalse()
        {
            NullAuthCacheItem nullAuthCacheItem = new NullAuthCacheItem();
            nullAuthCacheItem.Valid().Should().BeFalse();
        }
    }
}