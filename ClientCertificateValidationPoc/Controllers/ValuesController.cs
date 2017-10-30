using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web.Http;
using ClientCertificateValidationPoc.Models;
using ClientCertificateValidationPoc.Security;

namespace ClientCertificateValidationPoc.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [CertificateAuthorization]
        public IEnumerable<string> Get()
        {
            CacheItem cacheItem = MemoryCache.Default.GetCacheItem("CertificateAuthorizationMessages");
            if (cacheItem == null) return new List<string>();

            return ((INotebook) cacheItem.Value).AllPages();
        }
    }
}
