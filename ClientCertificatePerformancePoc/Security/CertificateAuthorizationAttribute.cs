using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using Interception;

namespace ClientCertificatePerformancePoc.Security
{
    [AttributeUsage(AttributeTargets.All)]
    public class CertificateAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly IClientCertificateHeader _clientCertificateHeader;
        private readonly IChainPolicyCop _chainPolicyCop;

        public CertificateAuthorizationAttribute() : this(new CastleProxy<IChainPolicyCop>()) { }

        public CertificateAuthorizationAttribute(CastleProxy<IChainPolicyCop> castleProxy) : 
            this(new ClientCertificateHeader(), castleProxy.Interceptor(new ChainPolicyCop())) { }

        public CertificateAuthorizationAttribute(IChainPolicyCop chainPolicyCop) : 
            this( new ClientCertificateHeader(), chainPolicyCop) { }

        public CertificateAuthorizationAttribute(
            IClientCertificateHeader clientCertificateHeader,
            IChainPolicyCop chainPolicyCop)
        {
            _clientCertificateHeader = clientCertificateHeader;
            _chainPolicyCop = chainPolicyCop;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return _chainPolicyCop.Legal(_clientCertificateHeader.ClientCertificate(actionContext));
        }
    }
}