using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Castle.DynamicProxy;
using PIS.AutoEnt.Pattern;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 分布式事务，需要对MSDTC组件设置
    /// </summary>
    public class PISTransactionInterceptor : PISAbstractInterceptor
    {
        #region 成员属性

        private TransactionScope tranScope;

        #endregion

        #region 构造函数

        public PISTransactionInterceptor()
        {
        }

        #endregion

        #region PISAbstractInterceptor 成员

        /// <summary>
        /// 进入时
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnEntry(PISInterceptorArgs eventArgs)
        {
            PISTransactionAttribute transAttr = eventArgs.Attribute as PISTransactionAttribute;

            tranScope = transAttr.GetTransactionScope();
        }

        /// <summary>
        /// 出现异常
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnException(PISInterceptorArgs eventArgs)
        {
            base.OnException(eventArgs);
        }

        /// <summary>
        /// 方法成功时
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnSuccess(PISInterceptorArgs eventArgs)
        {
            if (tranScope != null)
            {
                tranScope.Complete();
            }

            base.OnSuccess(eventArgs);
        }

        /// <summary>
        /// 退出时
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnExit(PISInterceptorArgs eventArgs)
        {
            if (tranScope != null)
            {
                tranScope.Dispose();
            }

            base.OnExit(eventArgs);
        }

        #endregion
    }
}
