using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 应用系统异常
    /// </summary>
    public class PISSystemException : PISException
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public PISSystemException()
        {
        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="message">异常的消息</param>
        public PISSystemException(string message)
            : base(message)
        {
        }

        public PISSystemException(string subcode, string message)
            : base(message)
        {
            ErrorCode = ErrorCode.Get(ErrorCode.Category.System, subcode);
        }

        public PISSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected override void Initialize()
        {
            this.ErrorCode = ErrorCode.Get(ErrorCode.Category.System);
        }

        #endregion
    }
}
