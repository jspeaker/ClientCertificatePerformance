using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using ClientCertificatePerformancePoc.Controllers;

namespace ClientCertificatePerformancePoc.Tests.TestUtilities
{
    public class ActionContextBuilder
    {
        public HttpActionContext ActionContext()
        {
            HttpRequestContext requestContext = new HttpRequestContext();
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            HttpControllerDescriptor controllerDescriptor = new HttpControllerDescriptor(new HttpConfiguration(), "Values", typeof(ValuesController));
            HttpControllerContext controllerContext = new HttpControllerContext(requestContext, requestMessage, controllerDescriptor, new ValuesController());
            HttpActionContext actionContext = new HttpActionContext(controllerContext, new ReflectedHttpActionDescriptor());
            return actionContext;
        }
    }
}