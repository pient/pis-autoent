using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PIS.AutoEnt.Web;
using PIS.AutoEnt.Web.Api;
using PIS.AutoEnt.Portal.GlobalResources;

namespace PIS.AutoEnt.Portal.WebSite.Controllers.api
{
    public class ModuleSettingController : PortalApiControllerBase
    {
        public MenuItem GetListData()
        {
            MenuItem root = new MenuItem()
            {
                Id = SystemHelper.NewCombId(),
                Code = "M_ROOT",
                Path = "M_ROOT",
                PathLevel = 0,
                SortIndex = 1,
                Name = "Root",
                Status = "Enabled",
                leaf = false
            };

            MenuItem profile = new MenuItem()
            {
                Id = SystemHelper.NewCombId(),
                Code = "M_SETUP_MYPROFILE",
                PathLevel = 1,
                SortIndex = 1,
                Name = "个人设置",
                Status = "Enabled",
                leaf = true
            };

            root.Items.Add(profile);

            MenuItem setting = new MenuItem()
            {
                Id = SystemHelper.NewCombId(),
                Code = "M_SETUP_SYS",
                PathLevel = 1,
                SortIndex = 1,
                Name = "系统设置",
                Status = "Enabled",
                leaf = false
            };

            var module = new MenuItem()
            {
                Id = SystemHelper.NewCombId(),
                Code = "M_SETUP_MODULE",
                Path = "sys.setting.Module",
                PathLevel = 1,
                SortIndex = 1,
                Name = "模块设置",
                leaf = true
            };

            var orgauth = new MenuItem()
            {
                Id = SystemHelper.NewCombId(),
                Code = "M_SETUP_ORGAUTH",
                Name = "组织权限",
                leaf = false
            };

            var user = new MenuItem()
            {
                Id = SystemHelper.NewCombId(),
                Code = "M_SETUP_ORGAUTH_USER",
                Name = "人员管理",
                leaf = true
            };

            var group = new MenuItem()
            {
                Id = SystemHelper.NewCombId(),
                Code = "M_SETUP_ORGAUTH_GROUP",
                Name = "组管理",
                leaf = true
            };

            orgauth.Items.Add(user);
            orgauth.Items.Add(group);

            var workflow = new MenuItem()
            {
                Id = SystemHelper.NewCombId(),
                Code = "M_SETUP_WORKFLOW",
                Name = "工作流",
                leaf = true

            };

            setting.Items.Add(module);
            setting.Items.Add(orgauth);
            setting.Items.Add(workflow);

            root.Items.Add(setting);

            return root;
        }
    }
}
