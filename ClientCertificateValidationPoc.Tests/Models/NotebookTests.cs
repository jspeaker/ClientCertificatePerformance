using System;
using System.Collections.Generic;
using System.Linq;
using ClientCertificateValidationPoc.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificateValidationPoc.Tests.Models
{
    [TestClass]
    public class NotebookTests
    {
        [TestMethod, TestCategory("Unit")]
        public void WhenAddingNoteShouldNotThrow()
        {
            INotebook notebook = new Notebook();

            Action action = () =>
            {
                notebook.NewNote("note");
            };

            action.ShouldNotThrow();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenMultipleNotesShouldReturnCorrectPages()
        {
            INotebook notebook = new Notebook();
            notebook.NewNote("1");
            notebook.NewNote("2");

            List<string> actual = notebook.AllPages().ToList();

            actual.Count().Should().Be(2);
            actual.First().Should().Be("1");
            actual.Last().Should().Be("2");
        }
    }
}
