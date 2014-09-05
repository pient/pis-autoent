using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PIS.AutoEnt.App;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Portal.Models.Org;
using PIS.AutoEnt.Portal.Models.Page;
using PIS.AutoEnt.Portal.Models.Sys;
using PIS.AutoEnt.XData.DataStore;

namespace PIS.AutoEnt.Portal.WebSite
{
    public class FormDataMapperConfig
    {
        public static void Register()
        {
            RegisterPage();

            RegisterSetup();

            RegisterOrg();
        }

        #region Page

        private static void RegisterPage()
        {
            Mapper.CreateMap<RegDataItem, PageLayoutData>()
                .ForMember(d => d.Layout, opt => opt.MapFrom(s => s.Data));
        }

        #endregion

        #region Sys

        private static void RegisterSetup()
        {
            Mapper.CreateMap<ModuleForm, SysModule>();

            Mapper.CreateMap<RegisterForm, RegDataNode>();

            Mapper.CreateMap<RegisterForm, RegDataItem>();

            Mapper.CreateMap<RegDataNode, RegisterForm>()
                .ForMember(d => d.leaf, opt => opt.MapFrom(s => s.Nodes.Count == 0))
                .ForMember(d => d.ParentId, opt => opt.MapFrom(s => GetRegDataItemParentId(s)));

            Mapper.CreateMap<RegDataItem, RegisterForm>()
                .ForMember(d => d.ParentId, opt => opt.MapFrom(s => GetRegDataItemParentId(s)));
        }

        /// <summary>
        /// Used by mapping from RegDataNode to RegisterForm
        /// </summary>
        /// <param name="regNode"></param>
        /// <returns></returns>
        private static Guid? GetRegDataItemParentId(XDataItem<RegDataItem> regItem)
        {
            if (regItem.ParentNode == null)
            {
                return null;
            }
            else
            {
                return regItem.ParentNode.Id;
            }
        }

        #endregion

        #region Org

        private static void RegisterOrg()
        {
            Mapper.CreateMap<UserForm, OrgUser>();
        }

        #endregion
    }
}