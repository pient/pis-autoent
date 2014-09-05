using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PIS.AutoEnt.App;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Portal.Models.Sys;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.Web.Api;
using PIS.AutoEnt.XData.DataStore;

namespace PIS.AutoEnt.Portal.WebSite.Controllers.api.setup
{
    public class RegistrySetupController : PortalApiControllerBase
    {
        #region Module Setup

        [RequireAuthorize]
        public object GetNodes(string node, bool isparent = true)
        {
            var pageData = new object();

            if (String.IsNullOrEmpty(node) || node == "root")
            {
                var sysNode = AppRegistry.SystemNode;

                var regList = new List<RegisterForm>();

                var regFrm = AutoMapper.Mapper.Map<RegisterForm>(sysNode);
                regFrm.expanded = true; // 默认展开根节点
                regList.Add(regFrm);

                foreach (var n in sysNode.Nodes)
                {
                    var frm = AutoMapper.Mapper.Map<RegisterForm>(n);
                    regList.Add(frm);
                }

                pageData = regList;
            }
            else
            {
                var pNodeId = node.ToGuid();

                if (pNodeId != null)
                {
                    var pNode = AppRegistry.GetSysNodeById(pNodeId.Value);

                    pageData = pNode.Nodes.OrderBy(n=>n.SortIndex).Select((n) => {
                        var regFrm = AutoMapper.Mapper.Map<RegisterForm>(n);
                        return regFrm;
                    }).ToList();
                }
            }

            return pageData;
        }

        [RequireAuthorize]
        public object GetItems(string node)
        {
            var pageData = new object();

            var pNodeId = node.ToGuid();

            if (pNodeId != null)
            {
                var pNode = AppRegistry.GetSysNodeById(pNodeId.Value);

                pageData = pNode.Items.OrderBy(n => n.SortIndex).Select((n) =>
                {
                    var regFrm = AutoMapper.Mapper.Map<RegisterForm>(n);
                    return regFrm;
                }).ToList();
            }

            return pageData;
        }

        [RequireAuthorize]
        public void DeleteRegister(string node)
        {
            var id = node.ToGuid();

            if (!id.IsNullOrEmpty())
            {
                AppRegistry.System.RemoveById(id.Value);

                AppRegistry.PersistSystem();
            }
        }

        [RequireAuthorize]
        public void SaveRegister(RegisterForm regForm, string nodeType = "Node")
        {
            if (regForm.Id.IsNullOrEmpty())
            {
                if (!regForm.ParentId.IsNullOrEmpty())
                {
                    var pNode = AppRegistry.GetSysNodeById(regForm.ParentId.Value);

                    if (pNode != null)
                    {
                        if (nodeType.Equals("Item", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var regItem = AutoMapper.Mapper.Map<RegDataItem>(regForm);

                            AppRegistry.System.NewItem(pNode, regItem);
                        }
                        else
                        {
                            var regNode = AutoMapper.Mapper.Map<RegDataNode>(regForm);
                            AppRegistry.System.NewNode(pNode, regNode);
                        }
                    }
                }
            }
            else
            {
                var regObj = AppRegistry.GetSysObjById(regForm.Id.Value);

                if (regObj is RegDataNode)
                {
                    CLRHelper.MergeObject(regObj as RegDataNode, regForm);
                }
                else if (regObj is RegDataItem)
                {
                    CLRHelper.MergeObject(regObj as RegDataItem, regForm);
                }
            }

            AppRegistry.PersistSystem();
        }

        /// <summary>
        /// 持久化数据更改到数据库
        /// </summary>
        [RequireAuthorize]
        public void PersistRegistry()
        {
            AppRegistry.PersistSystem();
        }

        #endregion
    }
}
