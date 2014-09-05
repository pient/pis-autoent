using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using PIS.AutoEnt.Pattern;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 日志拦截器
    /// </summary>
    public class PISLoggingInterceptor : PISAbstractInterceptor
    {
        public override void OnSuccess(PISInterceptorArgs args)
        {
            if (args.Attribute != null)
            {
                PISLoggingAttribute attr = args.Attribute as PISLoggingAttribute;

                if (attr != null)
                {
                    LogManager.Log(attr.Message, attr.Level, attr.LoggerName);
                }
            }
        }

        public override void OnException(PISInterceptorArgs args)
        {
            if (args.Attribute != null)
            {
                PISLoggingAttribute attr = args.Attribute as PISLoggingAttribute;

                if (attr != null)
                {
                    String message = attr.Message;

                    if (attr.AppendUserInfo)
                    {
                        if (String.IsNullOrEmpty(message))
                        {
                            message = String.Empty;
                        }

                        if (SysPrincipal.CurrentUser != null)
                        {
                            message += " Executed by: " + SysPrincipal.CurrentUser.Name;
                        }
                    }

                    LogManager.Log(message, args.Exception, attr.Level, attr.LoggerName);
                }

                throw args.Exception;
            }
        }
    }
}
