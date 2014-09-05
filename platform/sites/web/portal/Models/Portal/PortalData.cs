using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Portal.Models.Home;
using PIS.AutoEnt.Web;

namespace PIS.AutoEnt.Portal.Models
{
    public class PortalData : PageData
    {
        #region Properties

        public PortalUser User { get; set; }

        public ModuleItem Menu
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public PortalData()
        {
            Tag = new EasyDictionary();

            var appUser = AppSecurity.GetCurrentUser();

            if (appUser != null)
            {
                User = new PortalUser(appUser);
            }
        }

        #endregion
    }
}
