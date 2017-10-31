using ClientCertificatePerformancePoc.Security;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Security
{
    [TestClass]
    public class NullAuthCacheItemTests
    {
        [TestMethod, TestCategory("Unit")]
        public void PopulatedShouldReturnFalse()
        {
            IAuthCacheItem nullAuthCacheItem = new NullAuthCacheItem();
            nullAuthCacheItem.Populated().Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void ValidShouldReturnFalse()
        {
            IAuthCacheItem nullAuthCacheItem = new NullAuthCacheItem();
            nullAuthCacheItem.Valid().Should().BeFalse();
        }
    }
}