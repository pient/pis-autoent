using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Web.Routing;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt.Web.Mvc
{
    /// <summary>
    /// PIS一般继承的Controller,需要身份验证
    /// </summary>
    public class PortalControllerBase : AbstractController
    {
        #region 属性成员

        public AppUser CurrentUser
        {
            get
            {
                return AppPortal.CurrentUser;
            }
        }

        #endregion
    }
}
