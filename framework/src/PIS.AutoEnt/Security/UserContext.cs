using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt
{
    /// <summary>
    /// user status
    /// </summary>
    public enum UserStatus
    {
        Online,
        Away,
        Busy,
        BeRightBack,
        Hidden,
        Idle,
        Offline
    }

    /// <summary>
    /// 用户上下文(参考HttpContext设计理念, 从用户登录系统开始保持到用户退出系统销毁)
    /// 用户登录系统后可随时随地调用
    /// </summary>
    public class UserContext : ContextBase
    {
        #region Fields

        public const string APP_USER = "APP_USER";
        public const string USER_STATUS = "USER_STATUS";

        #endregion

        #region Constructors

        public UserContext()
        {
        }

        #endregion

        #region Properties

        public UserInfo AppUser
        {
            get { return base.Get(APP_USER) as UserInfo; }
            internal set { base.Set(APP_USER, value); }
        }

        /// <summary>
        /// User Status
        /// </summary>
        public UserStatus UserStatus
        {
            get
            {
                return CLRHelper.GetEnum<UserStatus>(base.Get(USER_STATUS), UserStatus.Offline);
            }
            internal set { base.Set(USER_STATUS, value); }
        }

        #endregion

        #region Static Members

        /// <summary>
        /// 获取当前用户(不是很靠谱，Web应用中CurrentPrincipal无法获取用户权限信息)
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetCurrentUser()
        {
            SysPrincipal principal = null;

            if (HttpContext.Current != null)    // for web application
            {
                principal = HttpContext.Current.User as SysPrincipal;
            }
            else
            {
                principal = Thread.CurrentPrincipal as SysPrincipal;
            }

            if (principal != null)
            {
                SysIdentity identity = principal.Identity as SysIdentity;

                if (identity != null)
                {
                    return identity.User;
                }
            }

            return null;
        }

        #endregion
    }
}
