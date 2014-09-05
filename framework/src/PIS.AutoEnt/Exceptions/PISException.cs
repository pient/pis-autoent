using System;
using System.Collections.Generic;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// PIS异常
    /// </summary>
    public class PISException : Exception
    {
        #region 属性

        /// <summary>
        /// 错误编码
        /// </summary>
        public ErrorCode ErrorCode
        {
            get;
            protected set;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public PISException()
        {
            Initialize();
        }

        public PISException(Exception innerException)
            : base(innerException.Message, innerException)
        {
            Initialize();
        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="message">异常的消息</param>
        public PISException(string message)
            : base(message)
        {
            Initialize();
        }

        public PISException(string message, Exception innerException)
            : base(message, innerException)
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            this.ErrorCode = ErrorCode.Get(ErrorCode.Category.General);
        }

        #endregion
    }
}
