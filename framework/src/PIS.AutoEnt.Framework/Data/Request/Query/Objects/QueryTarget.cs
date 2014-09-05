using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    /// <summary>
    /// 查询对象类型
    /// </summary>
    public enum QueryTargetType
    {
        Table,
        View,
        TableOrView,
        QueryString,
        StoreProcedure
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    public class QueryTarget
    {
        #region 属性

        /// <summary>
        /// 查询对象字符串，可以是 表，视图，查询字符串或存储过程
        /// </summary>
        public string String
        {
            get;
            protected set;
        }

        /// <summary>
        /// 查询对象类型 
        /// </summary>
        public QueryTargetType Type
        {
            get;
            protected set;
        }

        public string Alias
        {
            get;
            protected set;
        }

        #endregion

        #region 构造函数

        public QueryTarget(string str, QueryTargetType type = QueryTargetType.TableOrView, string alias = "__a")
        {
            this.String = str;
            this.Type = type;
            this.Alias = alias;
        }

        #endregion
    }
}
