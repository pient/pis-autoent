using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    public static class CLRExtensions
    {
        #region 类型操作

        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return (guid == null || guid == Guid.Empty);
        }

        #endregion

    }
}
