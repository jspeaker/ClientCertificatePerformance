using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Logging.Destinations;
using Logging.Verbosity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Logging
{
    [TestClass]
    public class NotebookTests
    {
        [TestMethod, TestCategory("Unit")]
        public void WhenAddingNoteShouldNotThrow()
        {
            Notebook notebook = new Notebook();

            Action action = () =>
            {
                notebook.WriteEntry("Values", "note", new EventType("Information"));
            };

            action.ShouldNotThrow();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenMultipleNotesShouldReturnCorrectPages()
        {
            Notebook notebook = new Notebook();
            notebook.WriteEntry("Values", "1", new EventType("Information"));
            notebook.WriteEntry("Values", "2", new EventType("Information"));

            List<string> actual = notebook.Print().ToList();

            actual.Count().Should().Be(2);
            actual.First().Should().Be("Values : 1 : Information");
            actual.Last().Should().Be("Values : 2 : Information");
        }
    }
}
