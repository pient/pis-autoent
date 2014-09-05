using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 用户信息，用于Portal上下文
    /// </summary>
    [Serializable]
    public class PortalUser : UserInfo
    {
        #region 构造函数

        public PortalUser(string sessionID)
        {
            this.SessionID = sessionID;

            // 从WebService获取用户信息
        }

        public PortalUser(UserInfo ui)
        {
            this.SessionID = ui.SessionID;
            this.UserID = ui.UserID;
            this.Name = ui.Name;
            this.LoginName = ui.LoginName;
            this.SecurityLevel = ui.SecurityLevel;
            this.LastAccessed = ui.LastAccessed;
            this.Language = ui.Language;
        }

        #endregion
    }
}
