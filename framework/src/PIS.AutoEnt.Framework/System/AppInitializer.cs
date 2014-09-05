using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt.Sys
{
    public static class AppInitializer
    {
        #region Initialize System

        private static bool sysInitialized = false;
        private static object sysLockObj = new object();

        /// <summary>
        /// 初始化系统
        /// </summary>
        public static bool Initialize()
        {
            lock (sysLockObj)
            {
                if (sysInitialized == false)
                {
                    lock (sysLockObj)
                    {
                        // 初始化日志配置
                        LogManager.Configure();

                        LogManager.Log("Initializing System...");

#if !DEBUG
                        // 先检查系统是否有效(放在系统初始化这里，以免影响业务系统)
                        AppSystem.VerifySystem();
#endif

                        // 初始化基本配置信息
                        ObjectFactory.Configure();

                        // 初始化数据访问配置
                        // NHibernateManager.Configure();

                        sysInitialized = true;

                        LogManager.Log("System Initialized");
                    }

                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
