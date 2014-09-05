using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 应用上下文(参考HttpContext设计理念, 从用户登录系统开始保持到用户退出系统销毁)
    /// 用户登录系统后可随时随地调用
    /// </summary>
    public class AppContext
    {
        #region 成员变量

        /// <summary>
        /// 单前上下文，与当前用户相关
        /// </summary>
        public static AppContext Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region 构造函数，单体模式

        static AppContext instance;

        internal static AppContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppContext();
                }

                return instance;
            }
        }

        protected AppContext()
        {
        }

        #endregion
    }
}
