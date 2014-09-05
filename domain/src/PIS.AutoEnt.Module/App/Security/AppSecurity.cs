using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt
{
    public static class AppSecurity
    {
        /// <summary>
        /// Get Current User From User Session
        /// </summary>
        /// <returns></returns>
        public static AppUser GetCurrentUser()
        {
            AppUser currentUser = null;

            if (SysPrincipal.CurrentUser != null)
            {
                var _us = UserSession.Get(SysPrincipal.CurrentUser.SessionId);

                currentUser = _us.UserInfo;
            }

            return currentUser;
        }

        public static OrgUser GetUser(Guid userId)
        {
            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                OrgUser user = repo.Find(userId);

                return user;
            }
        }

        public static OrgUser GetUser(string userName)
        {
            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                OrgUser user = repo.Find(e => e.Code == userName);

                return user;
            }
        }

        public static OrgUser CreateAccount(DtoModels.UserAccount userAccount)
        {
            OrgUser user = AutoMapper.Mapper.Map<OrgUser>(userAccount);

            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                user = repo.Create(user);

                repo.EntityContext.SaveChanges();

                repo.ChangePassword(user, userAccount.Password);
            }

            return user;
        }

        public static void CloseAccount(Guid userId)
        {
            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                OrgUser user = repo.Find(userId);

                if (user != null)
                {
                    user.Status = OrgUser.UserStatus.Disabled;

                    repo.Update(user);
                }
            }
        }

        public static void CloseAccount(string userName)
        {
            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                OrgUser user = repo.Find(e => e.Code == userName);

                if (user != null)
                {
                    user.Status = OrgUser.UserStatus.Disabled;

                    repo.Update(user);
                }
            }
        }

        /// <param name="userName"></param>
        public static void RemoveAccount(Guid userId)
        {
            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                OrgUser user = repo.Find(userId);

                if (user != null)
                {
                    repo.Delete(user);
                }
            }
        }

        /// <summary>
        /// Remove all user infomation permanently, be careful about this
        /// use CloseAccount normally.
        /// </summary>
        /// <param name="userName"></param>
        public static void RemoveAccount(string userName)
        {
            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                OrgUser user = repo.Find(e => e.Code == userName);

                if (user != null)
                {
                    repo.Delete(user);
                }
            }
        }

        public static UserSession Login(DtoModels.AuthInfo authInfo)
        {
            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                OrgUser user = VerifyAuth(repo, authInfo);

                if (user != null)
                {
                    authInfo.UserId = user.Id;

                    UserSession userSession = new UserSession(user.Id);
                    // UserSession userSession = new UserSession(user);    // PIS-MOCK

                    UserSession.Set(userSession);

                    AppSystem.AppProvider.Logon(userSession.UserInfo, authInfo);

                    return userSession;
                }

                return null;
            }
        }

        public static bool ChangePassword(DtoModels.AuthInfo authInfo, bool verifyFirst = true)
        {
            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                OrgUser user = null;

                if (verifyFirst == true)
                {
                    user = VerifyAuth(repo, authInfo);
                }
                else
                {
                    user = repo.FindByLoginName(authInfo.LoginName);
                }

                if (user != null)
                {
                    repo.ChangePassword(user, authInfo.NewPassword);
                    repo.Update(user);

                    return true;
                }

                return false;
            }
        }

        public static void Logout()
        {
            if (SysPrincipal.CurrentUser != null)
            {
                UserSession.Release(SysPrincipal.CurrentUser.SessionId);
            }

            AppSystem.AppProvider.Logoff();
        }

        public static void Logout(string userId)
        {
            UserSession userSession = UserSession.GetByUserId(userId);

            UserSession.Release(userSession.SessionId);
        }

        #region Private Methods

        private static OrgUser VerifyAuth(IUserRepository repo, DtoModels.AuthInfo authInfo)
        {
            // 验证用户名是否为空
            if (String.IsNullOrEmpty(authInfo.LoginName))
            {
                throw new AuthException(AuthFailedReason.EmptyName);
            }

            OrgUser user = repo.Find(p => p.LoginName == authInfo.LoginName);

            if (user == null)
            {
                throw new AuthException(AuthFailedReason.NotExist);
            }

            if (!repo.VerifyPassword(user, authInfo.Password))
            {
                throw new AuthException(AuthFailedReason.InvalidPassword);
            }

            return user;
        }

        #endregion
    }
}
