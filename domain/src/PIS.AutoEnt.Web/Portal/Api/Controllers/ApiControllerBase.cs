using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Collections.Specialized;

namespace PIS.AutoEnt.Web.Api
{
    public class ControllerBase : ApiController
    {
        #region Controller 成员

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            // 设置用户语言
            String lang = AppPortal.GetLanguage();

            if (!String.IsNullOrEmpty(lang))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            }
        }

        #endregion
    }
}
