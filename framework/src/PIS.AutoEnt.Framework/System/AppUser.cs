using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    [Serializable]
    public class AppUser : UserInfo
    {
        #region Constructors

        protected AppUser() { }

        public AppUser(UserInfo userInfo)
        {
            this.Initialize(userInfo);
        }

        protected void Initialize(UserInfo userInfo)
        {
            if (userInfo != null)
            {
                this.SessionId = userInfo.SessionId;
                this.UserId = userInfo.UserId;
                this.Name = userInfo.Name;
                this.LoginName = userInfo.LoginName;
                this.SecurityLevel = userInfo.SecurityLevel;
                this.LastAccessed = userInfo.LastAccessed;
                this.Language = userInfo.Language;
            }
        }

        #endregion
    }
}
