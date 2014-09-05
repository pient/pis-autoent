using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

namespace PIS.AutoEnt.Security
{
    public class SysIdentity : ISysIdentity
    {
        #region 私有函数

        private UserInfo user = null;

        #endregion

        #region 构造函数

        public SysIdentity(UserInfo ui)
        {
            this.user = ui;
        }

        #endregion

        #region ISysIdentity Members

        /// <summary>
        /// 认证信息
        /// </summary>
        public UserInfo User
        {
            get { return this.user; }
        }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get
            {
                if (user != null)
                {
                    return user.Name;
                }

                return null;
            }
        }

        #endregion
    }
}
