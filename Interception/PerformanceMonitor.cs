using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using Logging.Destinations;
using Logging.Verbosity;

namespace Interception
{
    public class PerformanceMonitor : IInterceptor
    {
        private readonly ILog[] _logs;

        public PerformanceMonitor() : this(new ApplicationInsightsLog()) { }

        public PerformanceMonitor(params ILog[] logs)
        {
            _logs = logs;
        }

        public void Intercept(IInvocation invocation)
        {
            LogStart(invocation);
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                LogException(invocation, e);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                LogPerformance(invocation, stopwatch);
            }
        }

        private void LogStart(IInvocation invocation)
        {
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            foreach (ILog log in _logs)
            {
                log.WriteEntry($"{invocation.TargetType}.{invocation.Method}",
                    $"{invocation.TargetType}.{invocation.Method} was called at {timeOfDay.Hours}:{timeOfDay.Minutes}:{timeOfDay.Seconds}.{timeOfDay.Milliseconds}.",
                     new EventType());
            }
        }

        private void LogPerformance(IInvocation invocation, Stopwatch stopwatch)
        {
            foreach (ILog log in _logs)
            {
                log.WriteEntry($"{invocation.TargetType}.{invocation.Method}",
                    $"{invocation.TargetType}.{invocation.Method} took {stopwatch.ElapsedMilliseconds} milliseconds.",
                    new EventType());
            }
        }

        private void LogException(IInvocation invocation, Exception e)
        {
            foreach (ILog log in _logs)
            {
                log.WriteEntry($"{invocation.TargetType}.{invocation.Method}",
                    $"{e.Message} : {e.StackTrace}",
                    new EventType(new EventTypes().Error()));
            }
        }
    }
}