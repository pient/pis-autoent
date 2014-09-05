using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt.Portal
{
    public abstract class AbstractPortalProvider : IPortalProvider
    {
        #region IPortalProvider 成员

        public virtual PortalUser GetCurrentUser()
        {
            UserInfo appUser = UserContext.GetCurrentUser();

            if (appUser != null)
            {
                return appUser as PortalUser;
            }

            return null;
        }

        public abstract PortalUser GetUser(string sessionID);
        public abstract UserSessionState GetSessionState(string sessionID);
        public abstract void RefreshSession(string sessionID);
        public abstract void ReleaseSession(string sessionID);
        public abstract void Login(IAuthPackage authPackage);
        public abstract void Logout(string sessionID);

        #endregion
    }
}
