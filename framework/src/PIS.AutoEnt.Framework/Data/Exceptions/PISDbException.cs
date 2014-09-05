using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    public class PISDbException : PISDataException
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public PISDbException()
        {
        }

        public PISDbException(DbOpStatus status)
        {
            ErrorCode = DbErrorCode.Get(status);
        }

        public PISDbException(DbOpStatus status, string message)
            : base(message)
        {
            ErrorCode = DbErrorCode.Get(status);
        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="message">异常的消息</param>
        public PISDbException(string message)
            : base(message)
        {
        }

        public PISDbException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected override void Initialize()
        {
            this.ErrorCode = new DbErrorCode();
        }

        #endregion
    }
}
