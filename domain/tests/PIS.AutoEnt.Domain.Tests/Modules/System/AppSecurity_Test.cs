using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.App;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Framework.Tests.Modules.Modules.System
{
    [TestClass]
    public class AppSecurity_Test
    {
        public const string TestLoginName = "Test";
        public const string TestUserName = "Ray";
        public const string TestPassword = "Test_123";
        public const string TestErrorPassword = "Test_ERROR";
        public const string TestNewPassword = "Test_234";

        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
        }

        [TestMethod]
        public void Domain_AuthUser_Test()
        {
            var userAccount = new DtoModels.UserAccount()
            {
                Code = TestLoginName,
                Name = TestUserName,
                Password = TestPassword
            };

            OrgUser user = AppSecurity.GetUser(userAccount.Code);

            if (user != null)
            {
                AppSecurity.RemoveAccount(user.Code);
            }
            
            user = AppSecurity.CreateAccount(userAccount);

            Assert.IsNotNull(user);

            var authInfo = new DtoModels.AuthInfo()
            {
                LoginName = TestLoginName,
                Password = TestErrorPassword
            };

            try
            {
                AppSecurity.Login(authInfo);
                Assert.Fail();
            }
            catch (AuthException aex)
            {
                Assert.AreEqual(aex.Reason, AuthFailedReason.InvalidPassword);
            }

            authInfo.Password = TestPassword;
            authInfo.NewPassword = TestNewPassword;
            AppSecurity.ChangePassword(authInfo);

            user = AppSecurity.GetUser(user.Code);

            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                var _u = repo.Find(e => e.Code == user.Code);
                Assert.IsTrue(repo.VerifyPassword(_u, TestNewPassword));
            }

            authInfo.Password = TestNewPassword;
            // UserSession userSession = AppSecurity.Login(authInfo);
            
            // Assert.IsNotNull(AppSecurity.GetCurrentUser());

            AppSecurity.Logout();

            // Assert.IsNull(AppSecurity.GetCurrentUser());

            user = AppSecurity.GetUser(user.Code);

            Assert.IsNotNull(user);

            AppSecurity.CloseAccount(user.Code);

            Assert.IsTrue(user.Status == OrgUser.UserStatus.Disabled);

            AppSecurity.RemoveAccount(user.Code);
            user = AppSecurity.GetUser(user.Code);

            Assert.IsNull(user);
        }
    }
}
