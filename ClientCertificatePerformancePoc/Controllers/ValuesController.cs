using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web.Http;
using ClientCertificatePerformancePoc.Logging;
using ClientCertificatePerformancePoc.Security;

namespace ClientCertificatePerformancePoc.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [CertificateAuthorization]
        public IEnumerable<string> Get()
        {
            CacheItem cacheItem = MemoryCache.Default.GetCacheItem("CertificateAuthorizationMessages");
            if (cacheItem == null) return new List<string>();

            return ((ILogDestination) cacheItem.Value).Print();
        }
    }
}
