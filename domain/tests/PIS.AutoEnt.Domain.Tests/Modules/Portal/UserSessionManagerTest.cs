using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Tests.Modules.Portal
{
    [TestClass]
    public class UserSessionManagerTest
    {
        const string TestUserName = "Test";
        const string TestUserCode = "Test";
        OrgUser orgUser = null;

        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
            
            orgUser = new OrgUser()
            {
                Name = TestUserName,
                Code = TestUserCode
            };

            using (var uow = SysDataAccessor.NewUnitOfWork())
            {
                var repo = AppDataAccessor.GetRepository<IUserRepository>(uow.Context);

                var orgUser2 = repo.FindByCode(orgUser.Code);

                if (orgUser2 != null)
                {
                    repo.Delete(orgUser2);
                }

                repo.Create(orgUser);

                uow.Commit();
            }
        }

        [TestCleanup]
        public void Clear()
        {
            using (var uow = SysDataAccessor.NewUnitOfWork())
            {
                AppDataAccessor.GetRepository<IUserRepository>(uow.Context).Delete(orgUser);

                uow.Commit();
            }
        }

        [TestMethod]
        public void Domain_UserSessionManager_ConfigTest()
        {
            Assert.IsNotNull(UserSessionConfig.Instance.CachePolicy);
        }

        [TestMethod]
        public void Domain_UserSessionManager_SessionOperationsTest()
        {
            UserSession userSession = new UserSession(orgUser.Id);

            UserSession.Set(userSession);

            userSession = UserSession.Get(userSession.SessionId);

            Assert.IsNotNull(userSession);

            UserSession.Release(userSession.SessionId);
            userSession = UserSession.Get(userSession.SessionId);

            Assert.IsNull(userSession);
        }
    }
}
