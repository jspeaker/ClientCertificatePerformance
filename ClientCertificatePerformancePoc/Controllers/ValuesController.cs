using System.Web.Http;
using ClientCertificatePerformancePoc.Security;

namespace ClientCertificatePerformancePoc.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [CertificateAuthorization]
        public string Get()
        {
            return "value";
        }
    }
}
