using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.App.Interfaces;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.Repository;

namespace PIS.AutoEnt.App
{
    public static class AppInitializer
    {
        #region Initialize Data

        [PISTransaction]
        public static void InitializeData()
        {
            if (!AppPortal.RegDataStore.IsAppInitialized)
            {
                InitializeOrg();

                AppPortal.RegDataStore.IsAppInitialized = true;

                AppPortal.RegDataStore.PersistSysRegion();
            }
        }

        [PISTransaction]
        public static void RollbackData()
        {
            if (AppPortal.RegDataStore.IsAppInitialized)
            {
                RollbackOrg();

                AppPortal.RegDataStore.IsAppInitialized = false;

                AppPortal.RegDataStore.PersistSysRegion();
            }
        }

        #region Support Methods

        #region Initialize and rollback organization

        const string AdminUserName = "admin";
        const string AdminUserCode = "admin";
        const string AdminPassword = "admin";

        const string SysGroupName = "系统组";
        const string SysGroupCode = "Sys";
        const string OrgGroupName = "组织结构";
        const string OrgGroupCode = "Org";

        /// <summary>
        /// Initialize system organization
        /// </summary>
        static void InitializeOrg()
        {
            InitalizeRole();

            InitializeGroup();

            InitializeUser();
        }

        /// <summary>
        /// Rollback system organization
        /// </summary>
        static void RollbackOrg()
        {
            RollbackUser();
        }

        static void InitializeUser()
        {
            var userAccount = new DtoModels.UserAccount()
            {
                Code = AdminUserName,
                Name = AdminUserCode,
                Password = AdminPassword
            };

            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                var initUser = repo.FindByCode(userAccount.Code);

                if (initUser != null)
                {
                    repo.Delete(initUser);
                }
            }

            AppSecurity.CreateAccount(userAccount);
        }

        static void InitializeGroup()
        {
            using (var repo = AppDataAccessor.GetRepository<IGroupRepository>())
            {
                var initGroup = repo.FindByCode(SysGroupCode);

                if (initGroup != null)
                {
                    repo.Delete(initGroup);
                }

                initGroup = new OrgGroup()
                {
                    Code = SysGroupCode,
                    Name = SysGroupName,
                    Type = "System",
                    Status = "Enabled",
                };

                repo.CreateAsRoot(initGroup);

                initGroup = repo.FindByCode(OrgGroupCode);

                if (initGroup != null)
                {
                    repo.Delete(initGroup);
                }

                initGroup = new OrgGroup()
                {
                    Code = OrgGroupCode,
                    Name = OrgGroupName,
                    Type = "Orgnization",
                    Status = "Enabled",
                };

                repo.CreateAsRoot(initGroup);
            }
        }

        static void InitalizeRole()
        {
            using (var repo = AppDataAccessor.GetRepository<IRoleRepository>())
            {
                var initRole = repo.FindByCode("Administrators");

                if (initRole != null)
                {
                    repo.Delete(initRole);
                }

                initRole = new OrgRole()
                {
                    Code = "Administrators",
                    Name = "管理员",
                    Type = "System",
                    Status = "Enabled",
                };

                repo.Create(initRole);

                initRole = repo.FindByCode("Users");

                if (initRole != null)
                {
                    repo.Delete(initRole);
                }

                initRole = new OrgRole()
                {
                    Code = "Users",
                    Name = "用户",
                    Type = "System",
                    Status = "Enabled",
                };

                repo.Create(initRole);
            }
        }

        static void RollbackUser()
        {
            AppSecurity.RemoveAccount(AdminUserCode);
        }

        #endregion

        #endregion

        #endregion
    }
}
