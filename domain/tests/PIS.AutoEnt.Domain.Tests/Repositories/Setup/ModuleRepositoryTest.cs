using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Domain.Tests.Repositories.Setup
{
    [TestClass]
    public class ModuleRepositoryTest
    {
        private const string ROOT_CODE = "ROOT";
        private const string PARENT_CODE = "P_MODULE";
        private const string NORMAL_CODE = "N_MODULE";
        private const string SIB_CODE1 = "SIB_MODULE1";
        private const string SIB_CODE2 = "SIB_MODULE2";
        private const string SIB_CODE3 = "SIB_MODULE3";
        private const string SUB_CODE = "SUB_MODULE";

        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var repo = AppDataAccessor.GetRepository<IModuleRepository>())
            {
                var module = repo.FindByCode(ROOT_CODE);

                if (module != null)
                {
                    repo.Delete(module);
                }
            }
        }

        #region Structed Functions Test

        [TestMethod]
        public void ModuleRepositoryTest_Child_Operation_Test()
        {
            this.Cleanup();

            using (var repo = AppDataAccessor.GetRepository<IModuleRepository>())
            {
                var root = new SysModule()
                {
                    Code = ROOT_CODE,
                    Name = "根模块"
                };

                repo.CreateAsRoot(root);

                repo.DbContext.SaveChanges();

                var parent = new SysModule()
                {
                    Code = PARENT_CODE,
                    Name = "父模块"
                };

                repo.CreateAsChild(parent, root);

                repo.DbContext.SaveChanges();

                var normal = new SysModule()
                {
                    Code = NORMAL_CODE,
                    Name = "模块"
                };

                repo.CreateAsChild(normal, parent);

                repo.DbContext.SaveChanges();

                var sibling1 = new SysModule()
                {
                    Code = SIB_CODE1,
                    Name = "同级模块1"
                };

                repo.CreateAsSibling(sibling1, normal);

                repo.DbContext.SaveChanges();

                var sibling2 = new SysModule()
                {
                    Code = SIB_CODE2,
                    Name = "同级模块2"
                };

                repo.CreateAsSibling(sibling2, normal);

                repo.DbContext.SaveChanges();

                var sibling3 = new SysModule()
                {
                    Code = SIB_CODE3,
                    Name = "同级模块3"
                };

                repo.CreateAsChild(sibling3, parent, 2);

                repo.DbContext.SaveChanges();

                var sub = new SysModule()
                {
                    Code = SUB_CODE,
                    Name = "子模块"
                };

                repo.CreateAsChild(sub, normal);

                repo.DbContext.SaveChanges();
            }

            using (var repo = AppDataAccessor.GetRepository<IModuleRepository>())
            {
                var root = repo.FindByCode(ROOT_CODE);

                var ds = repo.GetDescendants(root);
                Assert.IsTrue(ds.Count == 6);

                var normal = repo.FindByCode(NORMAL_CODE);
                var _r = repo.GetAncestor(normal, 0);
                Assert.IsTrue(_r.Entity.Code == ROOT_CODE);

                var _p = repo.GetAncestor(normal, 1);
                Assert.IsTrue(_p.Entity.Code == PARENT_CODE);

                var _a = repo.GetAncestors(normal);
                Assert.IsTrue(_a.Count == 2);

                var _s = repo.GetStructure(normal);
                _r = repo.GetRoots(_s.PathLevel).FirstOrDefault();
                Assert.IsTrue(_r.Entity.Code == ROOT_CODE);

                var _children = repo.GetChildren(_p.Entity);
                Assert.IsTrue(_children.Count == 4);

                var _sibling1 = repo.FindByCode(SIB_CODE1);
                var _sibling2 = repo.FindByCode(SIB_CODE2);

                var _s_sibling1_index = repo.GetStructure(_sibling1).SortIndex;
                var _s_sibling2_index = repo.GetStructure(_sibling2).SortIndex;

                repo.ExchangeSiblingPosition(_sibling1, _sibling2);

                var _s_sibling3_index = repo.GetStructure(_sibling1).SortIndex;
                var _s_sibling4_index = repo.GetStructure(_sibling2).SortIndex;

                Assert.AreEqual(_s_sibling1_index, _s_sibling4_index);
                Assert.AreEqual(_s_sibling2_index, _s_sibling3_index);
            }
        }

        #endregion

        #region System Module

        // [TestMethod]
        public void Initialize_Sys_Module()
        {
            this.Cleanup();

            using (var repo = AppDataAccessor.GetRepository<IModuleRepository>())
            {
                var sys = repo.FindByCode(AppPortal.M_SYS_CODE);

                if (sys != null)
                {
                    repo.Delete(sys);
                }
            }

            using (var repo = AppDataAccessor.GetRepository<IModuleRepository>())
            {
                var sys = new SysModule()
                {
                    Code = AppPortal.M_SYS_CODE,
                    Name = "系统",
                    Status = SysModule.ModuleStatus.Enabled,
                    IsPublic = true
                };

                repo.CreateAsRoot(sys);

                var fav = new SysModule()
                {
                    Code = AppPortal.M_FAV_CODE,
                    Name = "收藏夹",
                    Status = SysModule.ModuleStatus.Enabled,
                    IsPublic = true
                };

                repo.CreateAsChild(fav, sys);

                var mdl = new SysModule()
                {
                    Code = AppPortal.M_MDL_CODE,
                    Name = "模块",
                    Status = SysModule.ModuleStatus.Enabled,
                    IsPublic = true
                };

                repo.CreateAsSibling(mdl, fav);

                var setup = new SysModule()
                {
                    Code = AppPortal.M_MDL_SETUP_CODE,
                    Name = "应用设置",
                    Status = SysModule.ModuleStatus.Enabled,
                    IsPublic = false
                };

                repo.CreateAsChild(setup, mdl);
            }
        }

        public void GetModules_Test()
        {
            
        }

        #endregion
    }
}
