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
using PIS.AutoEnt.Portal.Models;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Portal.Models.Home;

namespace PIS.AutoEnt.Portal.WebSite.Controllers.api
{
    public class PortalController : PortalApiControllerBase
    {
        // GET api/portal/GetPageData
        [RequireAuthorize]
        public object GetPageData(string code = null)
        {
            // 未登陆直接返回null
            if (UserSession.Current == null)
            {
                return null;
            }

            SysObjWithStructure<SysModule> module = null;

            if (!String.IsNullOrEmpty(code))
            {
                module = UserSession.Current.Modules.FirstOrDefault(e => e.Entity.Code == code);
            }

            // code为空返回根节点信息
            if (module == null || module.Structure.IsLeaf == false)
            {
                var pageData = new PortalData();
                var tree = new TreeNodes<SysModule>();

                if (module == null)
                {
                    tree = UserSession.Current.Modules.ToTree();
                }
                else
                {
                    tree = UserSession.Current.Modules.Where(e => e.Structure.ParentId == module.Entity.Id)
                        .ToList().ToTree();
                }

                if (tree.Count > 0)
                {
                    var _root = tree.First();

                    pageData.Menu = ModuleItem.FromMdlTree(_root);

                    // pageData.Menu = AppPortal.MapMenuFrom<MenuItem, SysModule>(_root);

                    // AppPortal.MapMenu<MenuItem, SysModule>(_root, pageData.Menu, 2);
                }

                return pageData;
            }
            else
            {
                var pageData = new PageData()
                {
                    Code = module.Entity.Code,
                    Title = module.Entity.Name,
                    Type = module.Entity.Type,
                    Path = module.Entity.MdlPath
                };

                return pageData;
            }
        }

        [RequireAuthorize]
        public object GetSysStructuredData(string name, string node = null, bool isparent = true, int? rootLevel = 0)
        {
            var pageData = new object();

            if (!String.IsNullOrEmpty(name))
            {
                var condition = "";

                if (String.IsNullOrEmpty(node) || node == "root")
                {
                    condition = "PathLevel=" + rootLevel;

                    pageData = SysDataAccessor.GetStructuredData(name, condition, null, 1);
                }
                else
                {
                    if (isparent == true)
                    {
                        condition = "ParentId='" + node + "'";
                    }
                    else
                    {
                        condition = "ObjectId='" + node + "'";
                    }

                    pageData = SysDataAccessor.GetStructuredData(name, condition);
                }
            }

            return pageData;
        }

        /// <summary>
        /// 重新加载系统模块
        /// </summary>
        [RequireAuthorize]
        public void ReloadMdlOrgAuth()
        {
            AppPortal.ReloadMdlOrgAuth();
        }

        /// <summary>
        /// 重新加载系统模块
        /// </summary>
        [RequireAuthorize]
        public void ReloadRegistry()
        {
            AppPortal.ResetRegistry();
        }
    }
}
