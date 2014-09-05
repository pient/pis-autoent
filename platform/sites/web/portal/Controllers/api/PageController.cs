using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Web;
using PIS.AutoEnt.Web.Api;
using PIS.AutoEnt.Portal.GlobalResources;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Portal.Models.Page;

namespace PIS.AutoEnt.Portal.WebSite.Controllers.api
{
    public class PageController : PortalApiControllerBase
    {
        [RequireAuthorize]
        public object GetLayout(string code = null)
        {
            var pageData = new object();

            var layoutNode = AppRegistry.GetSysNodeByCode(AppRegistry.Sys_Ptl_Layout_Code);

            if (layoutNode != null && !String.IsNullOrEmpty(code))
            {
                var layout = layoutNode.GetItemByCode(code);

                pageData = AutoMapper.Mapper.Map<PageLayoutData>(layout);
            }

            return pageData;
        }

        [RequireAuthorize]
        public object GetPortlet(string code)
        {
            return null;
        }
    }
}
