using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Portal;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Caching;
using System.Collections.ObjectModel;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.App;

namespace PIS.AutoEnt
{
    public class AppPortal
    {
        #region 常量

        public const string M_SYS_CODE = "M_SYS";
        public const string M_FAV_CODE = "M_FAV";
        public const string M_MDL_CODE = "M_MDL";
        public const string M_MDL_SETUP_CODE = "M_MDL.Setup";

        #endregion

        #region 变量

        public static object lockObj = new object();

        internal RegDataStore regDataStore;

        ReadOnlyCollection<SysModuleWithStructure> modules;
        ReadOnlyCollection<SysAuth> auths;
        ReadOnlyCollection<OrgGroup> groups;
        ReadOnlyCollection<OrgRole> roles;

        #endregion

        #region 属性

        /// <summary>
        /// 当前SessionID
        /// </summary>
        public static AppUser CurrentUser
        {
            get
            {
                AppUser appUser = AppSecurity.GetCurrentUser();

                return appUser;
            }
        }

        /// <summary>
        /// 系统注册信息
        /// </summary>
        internal static RegDataStore RegDataStore
        {
            get
            {
                if (Instance.regDataStore == null)
                {
                    lock (lockObj)
                    {
                        if (Instance.regDataStore == null)
                        {
                            Instance.InnerResetRegistry();
                        }
                    }
                }

                return Instance.regDataStore;
            }
        }

        /// <summary>
        /// 获取系统所有模块
        /// </summary>
        internal static IList<SysModuleWithStructure> Modules
        {
            get
            {
                if (Instance.modules == null)
                {
                    lock (lockObj)
                    {
                        if (Instance.modules == null)
                        {
                            Instance.InnerReloadMdlOrgAuth();
                        }
                    }
                }

                return Instance.modules;
            }
        }

        /// <summary>
        /// 获取系统所有权限
        /// </summary>
        internal static IList<SysAuth> Auths
        {
            get
            {
                if (Instance.auths == null)
                {
                    lock (lockObj)
                    {
                        if (Instance.auths == null)
                        {
                            Instance.InnerReloadMdlOrgAuth();
                        }
                    }
                }

                return Instance.auths;
            }
        }

        /// <summary>
        /// 获取系统所有角色
        /// </summary>
        internal static IList<OrgRole> Roles
        {
            get
            {
                if (Instance.roles == null)
                {
                    lock (lockObj)
                    {
                        if (Instance.roles == null)
                        {
                            Instance.InnerReloadMdlOrgAuth();
                        }
                    }
                }

                return Instance.roles;
            }
        }

        /// <summary>
        /// 获取系统所有组
        /// </summary>
        internal static IList<OrgGroup> Groups
        {
            get
            {
                if (Instance.groups == null)
                {
                    lock (lockObj)
                    {
                        if (Instance.groups == null)
                        {
                            Instance.InnerReloadMdlOrgAuth();
                        }
                    }
                }

                return Instance.groups;
            }
        }

        #endregion

        #region 构造函数

        private static readonly AppPortal Instance = new AppPortal();

        private AppPortal()
        {
            InnerReloadMdlOrgAuth();

            InnerResetRegistry();
        }

        #endregion

        #region 内存驻留对象管理

        /// <summary>
        /// 刷新应用组织模块权限
        /// </summary>
        [PISLogging("RefreshMdlOrgAuth")]
        internal void InnerReloadMdlOrgAuth()
        {
            LogManager.Log("Refreshing OrgMdlAuth");

            using (var uow = SysDataAccessor.NewUnitOfWork())
            {
                var ctx = uow.Context as SysDbContext;

                var _repo_module = AppDataAccessor.GetRepository<IModuleRepository>(ctx);

                modules = _repo_module.QueryModulesWithStructure(e => e.Status == SysModule.ModuleStatus.Enabled).ToList().AsReadOnly();

                auths = _repo_module.AuthRepository.FindAll(e => e.Status == SysModule.ModuleStatus.Enabled).ToList().AsReadOnly();

                var _repo_role = AppDataAccessor.GetRepository<IRoleRepository>(ctx);
                roles = _repo_role.FindAll().ToList().AsReadOnly();

                var _repo_user = AppDataAccessor.GetRepository<IGroupRepository>(ctx);

                groups = _repo_user.FindAll(e => e.Status == SysModule.ModuleStatus.Enabled).ToList().AsReadOnly();
            }

            LogManager.Log("Refreshed OrgMdlAuth");
        }

        public static void ReloadMdlOrgAuth()
        {
            Instance.InnerReloadMdlOrgAuth();
        }

        [PISLogging("ReloadRegistry")]
        internal void InnerResetRegistry()
        {
            LogManager.Log("Refreshing Registry");

            if (regDataStore == null)
            {
                regDataStore = new RegDataStore();
            }
            else
            {
                regDataStore.Reset();
            }

            LogManager.Log("Refreshed Registry");
        }

        public static void ResetRegistry()
        {
            Instance.InnerResetRegistry();
        }

        #endregion

        #region 国际化

        public static string GetLanguage()
        {
            return AppSystem.AppProvider.GetLanguage();
        }

        public static void SetLanguage(string lang)
        {
            AppSystem.AppProvider.SetLanguage(lang);
        }

        #endregion

        #region 帮助方法

        public static void MapMenu(TreeNode node, MenuItem menuItem, int? maxLevel = null)
        {
            MapMenu<MenuItem, object>(node, menuItem, maxLevel); 
        }

        public static MenuItem MapMenuFrom(TreeNode node, int? maxLevel = null)
        {
            return MapMenuFrom<MenuItem, object>(node, maxLevel);
        }

        public static void MapMenu<T1, T2>(TreeNode<T2> node, T1 menuItem, int? maxLevel = null)
            where T1 : MenuItem, new()
        {
            if (node.IsLeaf == false)
            {
                foreach (var _n in node.Items)
                {
                    var _m = MapMenuFrom<T1, T2>(_n, maxLevel);
                    menuItem.Items.Add(_m);

                    MapMenu<T1, T2>(_n, _m);
                }
            }
        }

        public static T1 MapMenuFrom<T1, T2>(TreeNode<T2> node, int? maxLevel = null)
            where T1 : MenuItem, new()
        {
            var item = new T1()
            {
                Id = node.Id,

                Code = node.Code,
                Name = node.Name,
                Path = node.Path,
                PathLevel = node.PathLevel,
                SortIndex = (int?)node.SortIndex,
                leaf = (node.PathLevel >= maxLevel ? true : node.IsLeaf),

                Tag = node.Tag
            };

            return item;
        }

        #endregion
    }
}
