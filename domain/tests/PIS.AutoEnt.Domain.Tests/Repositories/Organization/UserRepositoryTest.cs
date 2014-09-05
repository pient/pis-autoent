using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Framework.Tests.DataAccess.EF.Repositories.Organization
{
    [TestClass]
    public class UserRepositoryTest
    {
        const string TestUserName = "Test";
        const string TestUserCode = "Test";
        const string TestNewUserName = "Test_New";

        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
        }

        [TestMethod]
        public void UserRepository_CRUD()
        {
            OrgUser orgUser = new OrgUser()
            {
                Code = TestUserCode,
                Name = TestUserName
            };

            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                var orgUser2 = repo.FindByCode(TestUserCode);

                if (orgUser2 != null)
                {
                    repo.Delete(orgUser2);
                }
            }

            using (var uow = SysDataAccessor.NewUnitOfWork())
            {
                var repo = AppDataAccessor.GetRepository<IUserRepository>(uow.Context);

                repo.Create(orgUser);

                uow.Commit();
            }

            Assert.IsNotNull(orgUser.Id);

            orgUser.Name = TestNewUserName;

            using (var uow = SysDataAccessor.NewUnitOfWork())
            {
                var repo = AppDataAccessor.GetRepository<IUserRepository>(uow.Context);

                repo.Update(orgUser);

                orgUser = repo.Find(orgUser.Id);

                uow.Commit();
            }

            Assert.AreEqual(orgUser.Name, TestNewUserName);

            using (var uow = SysDataAccessor.NewUnitOfWork())
            {
                var repo = AppDataAccessor.GetRepository<IUserRepository>(uow.Context);

                repo.Delete(orgUser);

                uow.Commit();
            }

            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                orgUser = repo.Find(orgUser.Id);
            }

            Assert.IsNull(orgUser);
        }
    }
}
