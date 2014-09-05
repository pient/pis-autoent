using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Models;

namespace PIS.AutoEnt
{
    public class PortalManager
    {
        #region 成员属性

        protected IPortalProvider provider = null;

        /// <summary>
        /// 当前SessionID
        /// </summary>
        public static PortalUser CurrentUser
        {
            get
            {
                return Instance.provider.GetCurrentUser();
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 标识PortalService实例
        /// </summary>
        public string InstanceID { get; private set; }

        private static PortalManager instance = null;

        /// <summary>
        /// 单体模式
        /// </summary>
        internal static PortalManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PortalManager();
                }

                return instance;
            }
        }

        protected PortalManager()
        {
            this.InstanceID = Guid.NewGuid().ToString();
            provider = ObjectManager.Resolve<IPortalProvider>();
        }

        #endregion

        #region 公共方法属性

        public static void Initialize()
        {
            AppSystem.Initialize();
        }

        public static PortalUser GetUser(string sessionID)
        {
            return Instance.provider.GetUser(sessionID);
        }

        [PISLogging(Level = LogLevel.INFO, Message = "Invoke login", AppendUserInfo = true)]
        public static void Login(IAuthPackage authPackage)
        {
            Instance.provider.Login(authPackage);
        }

        /// <summary>
        /// 登出当前用户
        /// </summary>
        public static void Logout()
        {
            if (CurrentUser != null)
            {
                Logout(CurrentUser.SessionID);
            }
        }

        [PISLogging(Level = LogLevel.INFO, Message = "Invoke logout", AppendUserInfo = true)]
        public static void Logout(string sessionID)
        {
            Instance.provider.Logout(sessionID);
        }

        #endregion
    }
}
