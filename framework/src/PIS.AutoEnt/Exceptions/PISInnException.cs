using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 应用系统异常
    /// </summary>
    public class PISInnerException : PISException
    {
        #region 构造函数

        public PISInnerException(string innerErrorCode)
            : base()
        {
            ErrorCode = ErrorCode.GetInner(innerErrorCode);
        }

        #endregion
    }
}
