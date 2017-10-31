using System.Collections.Generic;
using System.Linq;
using ClientCertificatePerformancePoc.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Logging
{
    [TestClass]
    public class TelemetryClientWrapperTests
    {
        [TestMethod, TestCategory("Unit")]
        public void GivenTraceMessages_WhenCallingPrint_ThenItShouldReturnTheMessages()
        {
            ILogDestination logDestination = new TelemetryClientWrapper();
            logDestination.Trace("1");
            logDestination.Trace("2");

            List<string> actual = logDestination.Print().ToList();

            actual.Count.Should().Be(2);
            actual.First().Should().Be("1");
            actual.Last().Should().Be("2");
        }
    }
}