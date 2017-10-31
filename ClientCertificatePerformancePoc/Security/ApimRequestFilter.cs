using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using ClientCertificatePerformancePoc.Configuration;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace ClientCertificatePerformancePoc.Security
{
    public class ApimRequestFilterAttribute : ActionFilterAttribute
    {
        private readonly IConfiguration _configuration;

        public ApimRequestFilterAttribute() : this(new Configuration.Configuration()) { }

        public ApimRequestFilterAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Request.Headers.TryGetValues("apim-request-verification", out IEnumerable<string> apimRequestVerification);
            if (apimRequestVerification == null) ThrowNotFoundException();

            // ReSharper disable once AssignNullToNotNullAttribute
            List<string> apimRequestVerificationHeaders = apimRequestVerification.ToList();
            if (!apimRequestVerificationHeaders.First().Equals(_configuration.ApimRequestVerification())) ThrowNotFoundException();

            base.OnActionExecuting(actionContext);
        }

        private void ThrowNotFoundException()
        {
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}