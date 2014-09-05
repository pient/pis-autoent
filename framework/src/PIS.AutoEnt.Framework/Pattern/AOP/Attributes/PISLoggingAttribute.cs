using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using PIS.AutoEnt.Pattern;

namespace PIS.AutoEnt
{
    public class PISLoggingAttribute : PISAbstractedInterceptorAttribute
    {
        #region 成员属性 

        /// <summary>
        /// 日志名
        /// </summary>
        public string LoggerName
        {
            get;
            set;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 捕捉的异常类型
        /// </summary>
        public Type ExceptionType
        {
            get;
            set;
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level
        {
            get;
            set;
        }

        /// <summary>
        /// 是否添加用户信息
        /// </summary>
        public bool AppendUserInfo
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public PISLoggingAttribute(string loggerName = SysConsts.DefaultLoggerName)
        {
            this.LoggerName = loggerName;

            Level = LogLevel.DEBUG;
        }

        public PISLoggingAttribute(string message, string loggerName = SysConsts.DefaultLoggerName)
            : this(loggerName)
        {
            this.Message = message;
        }

        public PISLoggingAttribute(string message, Type exceptionType, string loggerName = SysConsts.DefaultLoggerName)
            : this(message, loggerName)
        {
            this.ExceptionType = exceptionType;
        }

        #endregion

        #region PISInterceptorAttribute 成员

        public override IPISInterceptor Interceptor
        {
            get
            {
                return new PISLoggingInterceptor();
            }
        }

        #endregion
    }
}
