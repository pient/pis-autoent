using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository;

namespace PIS.AutoEnt.Portal
{
    /// <summary>
    /// 用户信息，用于Portal上下文
    /// </summary>
    [Serializable]
    public class PortalUser : AppUser
    {
        #region 构造函数

        public PortalUser(OrgUser orgUser, string sessionId)
        {
            this.SessionId = sessionId;

            this.UserId = orgUser.Id.ToString();
            this.LoginName = orgUser.Code;
            this.Name = orgUser.Name;

            SysMetadata metadata = orgUser.GetMetadata();
            if (metadata != null)
            {
                this.SecurityLevel = metadata.SecurityLevel;
            }
        }

        public PortalUser(AppUser appUser)
        {
            this.Initialize(appUser);
        }

        #endregion
    }
}
