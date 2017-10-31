using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using ClientCertificatePerformancePoc.Controllers;
using FluentAssertions;
using Logging.Destinations;
using Logging.Verbosity;
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
        public void WhenCallingGet_ItShouldReturnValue()
        {
            ValuesController valuesController = new ValuesController();

            string actual = valuesController.Get();

            actual.Should().Be("value");
        }
    }
}