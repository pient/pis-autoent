using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PIS.AutoEnt.Data
{
    /// <summary>
    /// 数据域元数据
    /// </summary>
    public class FieldMeta
    {
        #region 成员属性

        public virtual string DataField
        {
            get;
            set;
        }

        public virtual string Table
        {
            get;
            set;
        }

        public virtual string ConnectionString
        {
            get;
            set;
        }

        public virtual string IdField
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public FieldMeta(string dataField, string table, string idField = null, string connString = null)
        {
            this.DataField = dataField;
            this.Table = table;
            this.IdField = idField ?? SysConsts.DefaultIdField;
            this.ConnectionString = connString ?? AppDataAccessor.ConnectionString;
        }

        #endregion
    }
}
