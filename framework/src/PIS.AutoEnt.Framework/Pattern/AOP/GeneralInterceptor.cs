using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace PIS.AutoEnt.Pattern
{
    public class GeneralInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
#if DEBUG
            Console.WriteLine("Begin General Intercept");
#endif

            IEnumerable<PISAbstractedInterceptorAttribute> attrs = 
                invocation.Method.GetCustomAttributes(typeof(PISAbstractedInterceptorAttribute), true).Select((v)=> {
                    return v as PISAbstractedInterceptorAttribute;
                });

            try
            {
                foreach (PISAbstractedInterceptorAttribute attr in attrs)
                {
                    attr.Interceptor.OnEntry(new PISInterceptorArgs(invocation, attr));
                }

                invocation.Proceed();

                foreach (PISAbstractedInterceptorAttribute attr in attrs)
                {
                    attr.Interceptor.OnSuccess(new PISInterceptorArgs(invocation, attr));
                }
            }
            catch (Exception ex)
            {
                foreach (PISAbstractedInterceptorAttribute attr in attrs)
                {
                    attr.Interceptor.OnException(new PISInterceptorArgs(invocation, attr, ex));
                }
            }
            finally
            {
                foreach (PISAbstractedInterceptorAttribute attr in attrs)
                {
                    attr.Interceptor.OnExit(new PISInterceptorArgs(invocation, attr));
                }
            }
            
#if DEBUG
            Console.WriteLine("After General Intercept");
#endif
        }
    }
}
