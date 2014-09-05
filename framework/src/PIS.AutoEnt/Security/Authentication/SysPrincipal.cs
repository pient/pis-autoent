using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Web;
using System.Threading;

namespace PIS.AutoEnt.Security
{
    public class SysPrincipal : IPrincipal
    {
        #region 属性

        public static UserInfo CurrentUser
        {
            get
            {
                return (CurrentIdentity == null ? null : CurrentIdentity.User);
            }
        }

        public static ISysIdentity CurrentIdentity
        {
            get
            {
                SysPrincipal _p = null;

                if (HttpContext.Current != null)    // for web application
                {
                    _p = HttpContext.Current.User as SysPrincipal;
                }
                else
                {
                    _p = Thread.CurrentPrincipal as SysPrincipal;
                }

                return (_p == null ? null : _p.Identity as ISysIdentity);
            }
        }

        #endregion

        #region 私有成员

        private ISysIdentity identity;

        #endregion

        #region 构造函数

        public SysPrincipal(UserInfo ui)
        {
            this.identity = new SysIdentity(ui);
        }

        public SysPrincipal(ISysIdentity identity)
        {
            this.identity = identity;
        }

        #endregion

        #region IPrincipal Members

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        #endregion

        #region 公有函数

        public UserInfo GetUser()
        {
            var identity = this.Identity as ISysIdentity;

            if (identity != null)
            {
                return identity.User;
            }

            return null;
        }

        #endregion
    }
}
