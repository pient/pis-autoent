using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    public class QueryField
    {
        #region 成员属性

        /// <summary>
        /// 域名
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 值数据类型
        /// </summary>
        public TypeCode TypeCode
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public QueryField()
            : this("")
        {
        }

        public QueryField(string fieldName)
            : this(fieldName, TypeCode.String)
        {
        }

        public QueryField(string fieldName, System.TypeCode typeCode)
        {
            this.Name = fieldName;
            this.TypeCode = typeCode;
        }

        #endregion
    }
}
