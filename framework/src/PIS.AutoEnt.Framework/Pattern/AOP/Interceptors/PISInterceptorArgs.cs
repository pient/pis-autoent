using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace PIS.AutoEnt.Pattern
{
    public class PISInterceptorArgs
    {
        public IInvocation Invocation
        {
            get;
            set;
        }

        public PISAbstractedInterceptorAttribute Attribute
        {
            get;
            set;
        }

        public Exception Exception
        {
            get;
            set;
        }

        public PISInterceptorArgs()
        {
        }

        public PISInterceptorArgs(IInvocation invocation, PISAbstractedInterceptorAttribute attribute)
        {
            this.Invocation = invocation;
            this.Attribute = attribute;
        }

        public PISInterceptorArgs(IInvocation invocation, PISAbstractedInterceptorAttribute attribute, Exception exception)
            : this(invocation, attribute)
        {
            this.Exception = exception;
        }
    }
}
