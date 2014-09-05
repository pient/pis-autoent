using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Portal.GlobalResources;
using PIS.AutoEnt.Web;
using PIS.AutoEnt.Web.Mvc;

namespace PIS.AutoEnt.Portal.Models
{
    public class PageModel : PortalViewModel
    {
        #region Properties

        public string PageCode { get; set; }

        #endregion

        #region Constructors

        public PageModel()
        {
        }

        public PageModel(string code)
        {
            this.PageCode = code;
        }

        #endregion
    }
}