using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace PIS.AutoEnt.Pattern
{
    public interface IPISInterceptor
    {
        void OnEntry(PISInterceptorArgs args);
        void OnSuccess(PISInterceptorArgs args);
        void OnExit(PISInterceptorArgs args);
        void OnException(PISInterceptorArgs args);
    }
}
