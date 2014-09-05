using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 系统安全异常
    /// </summary>
    public class PISSecException : PISException
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public PISSecException()
        {
        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="message">异常的消息</param>
        public PISSecException(string message)
            : base(message)
        {
        }

        public PISSecException(string subcode, string message)
            : base(message)
        {
            ErrorCode = ErrorCode.Get(ErrorCode.Category.System, subcode);
        }

        public PISSecException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected override void Initialize()
        {
            this.ErrorCode = ErrorCode.Get(ErrorCode.Category.Security);
        }

        #endregion
    }
}
