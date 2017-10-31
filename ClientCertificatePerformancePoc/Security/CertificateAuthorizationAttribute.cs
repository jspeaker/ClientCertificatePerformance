using System;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Web.Http;
using System.Web.Http.Controllers;
using ClientCertificatePerformancePoc.Logging;
using Microsoft.ApplicationInsights;

namespace ClientCertificatePerformancePoc.Security
{
    [AttributeUsage(AttributeTargets.All)]
    public class CertificateAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly IClientCertificateHeader _clientCertificateHeader;
        private readonly IChainPolicyCop _chainPolicyCop;
        private readonly ILogDestination _notebook;

        public CertificateAuthorizationAttribute() : 
            this(new Notebook(), new TelemetryClient(), new ClientCertificateHeader(), new ChainPolicyCop()) { }

        public CertificateAuthorizationAttribute(IChainPolicyCop chainPolicyCop) : 
            this(new Notebook(), new TelemetryClient(), new ClientCertificateHeader(), chainPolicyCop) { }

        public CertificateAuthorizationAttribute(ILogDestination notebook, 
            TelemetryClient telemetryClient, 
            IClientCertificateHeader clientCertificateHeader,
            IChainPolicyCop chainPolicyCop)
        {
            _notebook = notebook;
            _telemetryClient = telemetryClient;
            _clientCertificateHeader = clientCertificateHeader;
            _chainPolicyCop = chainPolicyCop;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                return _chainPolicyCop.Legal(_clientCertificateHeader.ClientCertificate(actionContext));
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                return false;
            }
            finally
            {
                stopwatch.Stop();
                _notebook.Trace($"Auth attribute took {stopwatch.ElapsedMilliseconds}ms.");
                MemoryCache.Default.Add("CertificateAuthorizationMessages", _notebook, new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
                });
            }
        }
    }
}