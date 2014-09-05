using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Portal.GlobalResources;
using PIS.AutoEnt.Web;
using PIS.AutoEnt.Web.Mvc;

namespace PIS.AutoEnt.Portal.Models.Home
{
    public class HomeModel : PageModel
    {
        #region Properties

        public string SystemName
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public HomeModel()
        {
            // 设置系统信息 
            SystemName = GlobalResource.SystemName;
        }

        #endregion

        #region Support Methods

        #endregion
    }
}