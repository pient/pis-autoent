using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.App.Interfaces;
using PIS.AutoEnt.Portal;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Framework.Tests
{
    public class TestAppProvider : IAppProvider
    {
        #region IAppProvider Members

        public void Logon(AppUser appUser, DtoModels.AuthInfo authInfo)
        {
        }

        public void Logoff()
        {
        }

        public AppUser GetLoggedUser()
        {
            return new PortalUser(
                new OrgUser(),
                SystemHelper.NewCombId().ToString());
        }

        public string GetLanguage()
        {
            return "zh-CN";
        }

        public void SetLanguage(string lang)
        {
        }

        #endregion
    }
}
