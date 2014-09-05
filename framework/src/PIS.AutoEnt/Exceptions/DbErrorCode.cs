using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 数据库操作状态枚举
    /// </summary>
    public enum DbOpStatus
    {
        Successful = 0,
        SystemError = 1,
        SqlError = 2,
        RecordExist = 3,
        RecordNotExist = 4,
        RecordHasModified = 5,
        Unspecified = 6
    }

    public class DbErrorCode : ErrorCode
    {
        #region 成员属性

        #endregion

        #region 构造函数

        public DbErrorCode() 
            : this(DbOpStatus.Unspecified)
        {
            this.Categroy = Category.Data;
        }

        public DbErrorCode(DbOpStatus status)
        {
            this.Code = "100001" + ((int)status > 10 ? status.ToString() : "0" + status.ToString());
            this.Description = status.ToString();
        }

        #endregion

        #region 静态函数

        public static DbErrorCode Get(DbOpStatus status)
        {
            return new DbErrorCode(status);
        }

        #endregion
    }
}
