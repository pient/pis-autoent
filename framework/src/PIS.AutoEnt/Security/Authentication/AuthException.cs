using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    public enum AuthFailedReason
    {
        EmptyName = 1,
        InvalidName = 2,
        NotExist = 4,
        InvalidPassword = 8,
        NotInitialized = 16,
        Disabled = 32,
        Unknown = 0
    }

    public class AuthException : PISSecException
    {
        #region 成员属性

        public AuthFailedReason Reason
        {
            get;
            private set;
        }

        #endregion

        #region 构造函数

        public AuthException()
        {
        }

        public AuthException(string message)
            : base(message)
        {
        }

        public AuthException(AuthFailedReason reason)
        {
            this.Reason = reason;
        }

        public AuthException(AuthFailedReason reason, string message)
            : base(message)
        {
            this.Reason = reason;
        }

        protected override void Initialize()
        {
            this.ErrorCode = ErrorCode.Get(ErrorCode.Category.Security);
        }

        #endregion
    }
}
