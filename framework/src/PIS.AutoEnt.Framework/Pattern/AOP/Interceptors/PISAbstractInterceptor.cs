using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using PIS.AutoEnt.Pattern;

namespace PIS.AutoEnt
{
    public abstract class PISAbstractInterceptor : IPISInterceptor
    {
        public virtual void OnEntry(PISInterceptorArgs args)
        {
            // Do nothing
        }

        public virtual void OnSuccess(PISInterceptorArgs args)
        {
            // Do nothing
        }

        public virtual void OnException(PISInterceptorArgs args)
        {
            // Do nothing
        }

        public virtual void OnExit(PISInterceptorArgs args)
        {
            // Do nothing
        }
    }
}
