using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data.Query;

namespace PIS.AutoEnt.Data
{
    public class QueryExpr
    {
        #region Consts

        public const int DefaultPageSize = 25;  // 设置每页默认的记录数，默认值为25条,

        #endregion

        #region 私有成员

        // 自动添加自动排序字段
        protected bool _autoSort = true;

        //是否消除重复行
        protected bool _distinct = false;

        //当前页
        protected int _start = 1;

        //每页记录条数
        protected int _limit = DefaultPageSize;

        //总记录数量
        protected int _recordCount = -1;

        //是否获取总记录数量
        protected bool _getTotalCount = false;

        /// <summary>
        /// 所有的查询字符串
        /// </summary>
        private QueryFieldCollection queryFields = new QueryFieldCollection();

        #endregion

        #region 属性成员

        /// <summary>
        /// 自动添加自动排序字段,默认true
        /// </summary>
        public virtual bool AutoSort
        {
            get { return _autoSort; }
            set { _autoSort = value; }
        }

        /// <summary>
        /// 是否获取总记录数，默认false
        /// </summary>
        public virtual bool GetTotalCount
        {
            get { return this._getTotalCount; }
            set { this._getTotalCount = value; }
        }

        /// <summary>
        /// 每页记录条数
        /// </summary>
        public virtual int Limit
        {
            get { return this._limit; }
            set { this._limit = value; }
        }

        /// <summary>
        /// 从第几条记录开始查询，默认为0
        /// </summary>
        public virtual int Start
        {
            get { return _start; }
            set { this._start = value; }
        }

        /// <summary>
        /// 记录数量
        /// </summary>
        public virtual int TotalCount
        {
            get
            {
                return this._recordCount;
            }
            set
            {
                this._recordCount = value;
            }
        }

        /// <summary>
        /// 是否Distinct查询
        /// </summary>
        public virtual bool Distinct
        {
            get { return _distinct; }
            set { this._distinct = value; }
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        public virtual QueryTarget Target
        {
            get;
            set;
        }

        /// <summary>
        /// JuncCondition查询表达式
        /// </summary>
        public JuncCondition Conditions
        {
            get;
            internal set;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public QryOrderCollection Sorters
        {
            get;
            internal set;
        }

        /// <summary>
        /// 查询字段列表
        /// </summary>
        public virtual QueryFieldCollection QueryFields
        {
            get { return this.queryFields; }
        }

        #endregion

        #region 构造函数

        public QueryExpr()
        {
            Sorters = new QryOrderCollection();
            Conditions = new JuncCondition(JuncType.And);
        }

        public QueryExpr(string targetName, QueryTargetType targetType = QueryTargetType.View)
            : this()
        {
            this.Target = new QueryTarget(targetName, targetType);
        }

        #endregion
    }
}
