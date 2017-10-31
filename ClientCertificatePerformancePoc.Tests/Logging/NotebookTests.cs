using System;
using System.Collections.Generic;
using System.Linq;
using ClientCertificatePerformancePoc.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Logging
{
    [TestClass]
    public class NotebookTests
    {
        [TestMethod, TestCategory("Unit")]
        public void WhenAddingNoteShouldNotThrow()
        {
            ILogDestination notebook = new Notebook();

            Action action = () =>
            {
                notebook.Trace("note");
            };

            action.ShouldNotThrow();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenMultipleNotesShouldReturnCorrectPages()
        {
            ILogDestination notebook = new Notebook();
            notebook.Trace("1");
            notebook.Trace("2");

            List<string> actual = notebook.Print().ToList();

            actual.Count().Should().Be(2);
            actual.First().Should().Be("1");
            actual.Last().Should().Be("2");
        }
    }
}
