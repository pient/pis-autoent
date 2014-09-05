using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using PIS.AutoEnt.Pattern;

namespace PIS.AutoEnt
{
    public class PISExceptionAttribute : PISAbstractedInterceptorAttribute
    {
        #region 成员属性 

        /// <summary>
        /// 捕捉的异常类型
        /// </summary>
        public Type ExceptionType
        {
            get;
            private set;
        }

        #endregion

        #region 构造函数

        public PISExceptionAttribute()
            : this(typeof(Exception))
        {
        }

        public PISExceptionAttribute(Type exceptionType)
        {
            this.ExceptionType = exceptionType;
        }

        #endregion

        #region PISInterceptorAttribute 成员

        public override IPISInterceptor Interceptor
        {
            get
            {
                return new PISExceptionInterceptor();
            }
        }

        #endregion
    }
}
