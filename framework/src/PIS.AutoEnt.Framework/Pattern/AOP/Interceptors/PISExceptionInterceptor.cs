using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using PIS.AutoEnt.Pattern;

namespace PIS.AutoEnt
{
    /// <summary>
    /// Not completed yet
    /// </summary>
    public class PISExceptionInterceptor : PISAbstractInterceptor
    {
        public override void OnEntry(PISInterceptorArgs args)
        {
            Console.WriteLine("Begin Exception");
        }

        public override void OnSuccess(PISInterceptorArgs args)
        {
            Console.WriteLine("After Exception");
        }
    }
}
