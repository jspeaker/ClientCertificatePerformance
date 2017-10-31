using Castle.DynamicProxy;
using Logging.Destinations;

namespace Interception
{
    public class CastleProxy<T> where T : class
    {
        private readonly IInterceptor _interceptor;

        public CastleProxy() : this(new PerformanceMonitor()) { }

        public CastleProxy(ILog log) : this(new PerformanceMonitor(log)) { }

        public CastleProxy(IInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        public T Interceptor(object target)
        {
            return (T)new ProxyGenerator().CreateInterfaceProxyWithTarget(typeof(T), target, _interceptor);
        }
    }
}