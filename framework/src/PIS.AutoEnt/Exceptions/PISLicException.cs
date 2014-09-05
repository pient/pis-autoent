using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    public class PISLicException : PISException
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public PISLicException()
        {
        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="message">异常的消息</param>
        public PISLicException(string message)
            : base(message)
        {
        }

        public PISLicException(string subcode, string message)
            : base(message)
        {
            ErrorCode = ErrorCode.Get(ErrorCode.Category.License, subcode);
        }

        public PISLicException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected override void Initialize()
        {
            this.ErrorCode = ErrorCode.Get(ErrorCode.Category.License);
        }

        #endregion
    }
}
