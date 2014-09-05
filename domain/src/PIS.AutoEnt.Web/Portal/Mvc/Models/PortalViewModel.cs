using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Web.Mvc
{
    public class PortalViewModel : AbstractViewModel
    {
        public AppUser User
        {
            get
            {
                return AppPortal.CurrentUser;
            }
        }
    }
}
