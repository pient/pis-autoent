using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIS.AutoEnt.Portal.GlobalResources;
using PIS.AutoEnt.Portal.Models;
using PIS.AutoEnt.Portal.Models.Home;
using PIS.AutoEnt.Web;
using PIS.AutoEnt.Web.Mvc;

namespace PIS.AutoEnt.Portal.WebSite.Controllers
{
    public class HomeController : PortalControllerBase
    {
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        [RequireAuthorize]
        public ActionResult Index(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                this.SetLanguage(lang);

                return RedirectToAction("Index");
            }

            var homeModel = new HomeModel();

            return View(homeModel);
        }

        [RequireAuthorize]
        public ActionResult SubPortal(string code)
        {
            return View(new PageModel(code));
        }

        [RequireAuthorize]
        public ActionResult StagePage(string code)
        {
            return View(new PageModel(code));
        }


        [RequireAuthorize]
        public ActionResult Workbench()
        {
            return View(new HomeModel());
        }

        /// <summary>
        /// 帮助中心
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult HelpCenter()
        {
            return View();
        }
    }
}
