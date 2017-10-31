using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using ClientCertificatePerformancePoc.Controllers;
using ClientCertificatePerformancePoc.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTests
    {
        [TestInitialize]
        public void Teardown()
        {
            MemoryCache.Default.Remove("CertificateAuthorizationMessages");
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenNoNotes_WhenCallingGet_ItShouldReturnEmptyList()
        {
            ValuesController valuesController = new ValuesController();
            IEnumerable<string> actual = valuesController.Get();

            actual.Count().Should().Be(0);
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenNotes_WhenCallingGet_ItShouldReturnPopulatedList()
        {
            ILogDestination notebook = new Notebook();
            notebook.Trace("1");
            notebook.Trace("2");
            MemoryCache.Default.Set("CertificateAuthorizationMessages", notebook, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(5)
            });
            ValuesController valuesController = new ValuesController();
            List<string> actual = valuesController.Get().ToList();

            actual.Count.Should().Be(2);
            actual.First().Should().Be("1");
            actual.Last().Should().Be("2");
        }
    }
}