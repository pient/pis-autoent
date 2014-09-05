using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Castle.DynamicProxy;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Pattern;

namespace PIS.AutoEnt
{
    public class PISTransactionAttribute : PISAbstractedInterceptorAttribute
    {
        #region 成员属性

        /// <summary>
        /// 隔离级别
        /// </summary>
        public IsolationLevel IsolationLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 事务超时时间
        /// </summary>
        public TimeSpan Timeout
        {
            get;
            set;
        }

        /// <summary>
        /// 事务选项
        /// </summary>
        public TransactionScopeOption ScopeOption
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public PISTransactionAttribute()
        {
        }

        #endregion

        #region 公共方法

        public TransactionScope GetTransactionScope()
        {
            TransactionOptions transOpt = new TransactionOptions();
            transOpt.IsolationLevel = this.IsolationLevel;
            transOpt.Timeout = this.Timeout;

            TransactionScope scope = new TransactionScope(ScopeOption, transOpt);

            return scope;
        }

        #endregion

        #region PISInterceptorAttribute 成员

        public override IPISInterceptor Interceptor
        {
            get
            {
                return new PISTransactionInterceptor();
            }
        }

        #endregion
    }
}
