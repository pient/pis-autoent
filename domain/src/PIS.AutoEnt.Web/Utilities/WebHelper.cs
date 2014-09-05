using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Web.Portal;

namespace PIS.AutoEnt.Web
{
    public static class WebHelper
    {
        internal static void OnWebAuthorization()
        {
            var formIdentity = HttpContext.Current.User.Identity as FormsIdentity;

            string uistr = formIdentity.Ticket.UserData;

            if (!String.IsNullOrEmpty(uistr))
            {
                var _ud = UserData.FromJsonString(uistr);
                var _usr = UserSession.GetUser(_ud.Sid);

                HttpContext.Current.User = new SysPrincipal(_usr);
            }
        }
    }
}
