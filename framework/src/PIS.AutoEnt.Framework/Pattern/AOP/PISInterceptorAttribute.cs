using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using PIS.AutoEnt.Pattern;

namespace PIS.AutoEnt
{
    public class PISInterceptorAttribute : Castle.Core.InterceptorAttribute
    {
        #region 构造函数

        public PISInterceptorAttribute(string componentKey = InterceptorTypes.General)
            : base(componentKey)
        {
        }

        public PISInterceptorAttribute(Type interceptorType)
            : base(interceptorType)
        {
        }

        #endregion
    }
}
