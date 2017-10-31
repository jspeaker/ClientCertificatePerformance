using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Logging.Destinations;
using Logging.Verbosity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Logging
{
    [TestClass]
    public class ApplicationInsightsLogTests
    {
        [TestMethod, TestCategory("Unit")]
        public void GivenTraceMessages_WhenCallingPrint_ThenItShouldReturnTheMessages()
        {
            ApplicationInsightsLog logDestination = new ApplicationInsightsLog();
            logDestination.WriteEntry("Values", "1", new EventType("Information"));
            logDestination.WriteEntry("Values", "2", new EventType("Information"));

            List<string> actual = logDestination.Print().ToList();

            actual.Count.Should().Be(2);
            actual.First().Should().Be("Values : 1 : Information");
            actual.Last().Should().Be("Values : 2 : Information");
        }
    }
}