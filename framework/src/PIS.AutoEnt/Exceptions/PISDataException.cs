using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    public class PISDataException : PISException
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public PISDataException()
        {
        }

        public PISDataException(Exception innerException)
            : base(innerException)
        {
        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="message">异常的消息</param>
        public PISDataException(string message)
            : base(message)
        {
        }

        public PISDataException(string subcode, string message)
            : base(message)
        {
            ErrorCode = ErrorCode.Get(ErrorCode.Category.Data, subcode);
        }

        public PISDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected override void Initialize()
        {
            this.ErrorCode = ErrorCode.Get(ErrorCode.Category.Data);
        }

        #endregion
    }
}
