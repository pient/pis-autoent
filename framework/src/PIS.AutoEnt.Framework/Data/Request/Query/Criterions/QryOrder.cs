using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data.Query
{
    public class QryOrder
    {
        #region 公共属性

        /// <summary>
        /// 字段信息
        /// </summary>
        public string FieldName { get; protected set; }

        /// <summary>
        /// 排序
        /// </summary>
        public bool Ascending { get; protected set; }

        #endregion

        #region 构造函数

        public QryOrder(string fieldName, bool ascending = true)
        {
            this.FieldName = fieldName;
            this.Ascending = ascending;
        }

        #endregion
    }
}
