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

namespace PIS.AutoEnt.Web.Api
{
    /// <summary>
    /// PIS PortalController,用于提供数据服务, 一般以Json格式或Html格式以Get方式访问
    /// </summary>
    [RequireAuthorizeAttribute]
    public class PortalApiControllerBase : ControllerBase
    {
        #region Controller 成员

        #endregion
    }
}
