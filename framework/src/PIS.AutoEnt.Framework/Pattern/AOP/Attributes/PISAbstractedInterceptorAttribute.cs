using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Pattern
{
    /// <summary>
    /// PIS拦截器，实现用于对方法及属性的拦截
    /// 使用时需要为所在类或其基类加属性 Castle.DynamicProxy.IInterceptor
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited=true)]
    public class PISAbstractedInterceptorAttribute : Attribute
    {
        private Type interceptorType;

        public PISAbstractedInterceptorAttribute()
        {
        }

        public PISAbstractedInterceptorAttribute(Type interceptorType)
        {
            this.interceptorType = interceptorType;
        }

        public virtual IPISInterceptor Interceptor
        {
            get
            {
                IPISInterceptor interceptor = null;

                if (interceptorType != null)
                {
                    interceptor = ObjectFactory.Container.Resolve(this.interceptorType) as IPISInterceptor;
                }

                return null;
            }
        }
    }
}
