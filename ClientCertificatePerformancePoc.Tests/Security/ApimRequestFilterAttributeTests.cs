using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using ClientCertificatePerformancePoc.Security;
using ClientCertificatePerformancePoc.Tests.Fakes;
using ClientCertificatePerformancePoc.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientCertificatePerformancePoc.Tests.Security
{
    [TestClass]
    public class ApimRequestFilterAttributeTests
    {
        private ActionContextBuilder _actionContextBuilder;

        [TestInitialize]
        public void Setup()
        {
            _actionContextBuilder = new ActionContextBuilder();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenCorrectHeaderValue_WhenExecutingFilter_ShouldNotThrow()
        {
            HttpActionContext actionContext = _actionContextBuilder
                .ActionContext()
                .WithApimRequestVerificationHeader("apimRequestVerification");
            ApimRequestFilterAttribute apimRequestFilterAttribute = new ApimRequestFilterAttribute(new FakeConfiguration());

            Action action = () =>
            {
                apimRequestFilterAttribute.OnActionExecuting(actionContext);
            };

            action.ShouldNotThrow();
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenIncorrectHeaderValue_WhenExecutingFilter_ShouldThrowNotFoundException()
        {
            HttpActionContext actionContext = _actionContextBuilder
                .ActionContext()
                .WithApimRequestVerificationHeader("incorrect value");
            ApimRequestFilterAttribute apimRequestFilterAttribute = new ApimRequestFilterAttribute();

            Action action = () =>
            {
                apimRequestFilterAttribute.OnActionExecuting(actionContext);
            };

            action.ShouldThrow<HttpResponseException>().Where(e => e.Response.StatusCode == HttpStatusCode.NotFound);
        }

        [TestMethod, TestCategory("Unit")]
        public void GivenNoHeaderValue_WhenExecutingFilter_ShouldThrowNotFoundException()
        {
            HttpActionContext actionContext = _actionContextBuilder.ActionContext();
            ApimRequestFilterAttribute apimRequestFilterAttribute = new ApimRequestFilterAttribute();

            Action action = () =>
            {
                apimRequestFilterAttribute.OnActionExecuting(actionContext);
            };

            action.ShouldThrow<HttpResponseException>().Where(e => e.Response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}