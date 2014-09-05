using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PIS.AutoEnt.Web.Mvc
{
    /// <summary>
    /// 不需要身份验证
    /// </summary>
    public abstract class AbstractController : Controller
    {
        #region Controller 成员

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // 设置用户语言
            String lang = AppPortal.GetLanguage();

            if (String.IsNullOrEmpty(lang))
            {
                lang = Request.UserLanguages[0];

                if (!String.IsNullOrEmpty(lang))
                {
                    this.SetLanguage(lang);
                }
            }

            if (!String.IsNullOrEmpty(lang))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            }
        }

        #endregion

        #region 私有方法

        protected void SetLanguage(string lang)
        {
            AppPortal.SetLanguage(lang);
        }

        #endregion
    }
}
