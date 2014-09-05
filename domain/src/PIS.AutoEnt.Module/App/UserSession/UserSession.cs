using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Caching;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Portal;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt
{
    public class UserSession
    {
        #region Consts

        public const string SessionKey = "UserSession";   // 块名，用与Cache, Session，配置文件的键或路径

        #endregion

        #region Members

        IList<OrgGroup> groups = null;
        IList<OrgRole> roles = null;

        IList<SysAuth> auths = null;
        SysModulesWithStructure modules = null;

        #endregion

        #region Properties

        public string SessionId
        {
            get;
            internal set;
        }

        public AppUser UserInfo { get; private set; }

        public IList<OrgGroup> Groups
        {
            get { return groups; }
        }

        public IList<OrgRole> Roles
        {
            get { return roles; }
        }

        public IList<SysAuth> Auths
        {
            get { return auths; }
        }

        public SysModulesWithStructure Modules
        {
            get { return modules; }
        } 

        /// <summary>
        /// 扩展数据
        /// </summary>
        public EasyDictionary Tag { get; private set; }

        #endregion

        #region Constructors

        internal UserSession(Guid userId)
        {
            groups = new List<OrgGroup>();
            roles = new List<OrgRole>();

            auths = new List<SysAuth>();
            modules = new SysModulesWithStructure();

            OrgUser user = null;

            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                user = repo.Find(userId);
            }

            Initialize(user);
        }

        /// <summary>
        /// 备用方法，用于为任何用户构建Session --------- PIS-MOCK
        /// </summary>
        /// <param name="orgUser"></param>
        internal UserSession(OrgUser orgUser)
        {
            Initialize(orgUser);
        }

        private void Initialize(OrgUser orgUser)
        {
            this.Tag = new EasyDictionary();

            this.SessionId = NewSessionId();

            this.UserInfo = new PortalUser(orgUser, this.SessionId);

            this.Refresh();

        }

        #endregion

        #region Methods

        internal void Refresh()
        {
            LogManager.Log("Refreshing UserSession");

            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                var orgUser = repo.Find(this.UserInfo.UserId.ToGuid());

                // 获取并缓存用户所有组和角色
                var _groupids = orgUser.OrgGroups.Select(e => e.Id);
                groups = AppPortal.Groups.Where(e => _groupids.Contains(e.Id)).ToList().AsReadOnly();

                var _roleids = orgUser.OrgRoles.Select(e => e.Id);
                roles = AppPortal.Roles.Where(e => _roleids.Contains(e.Id)).ToList().AsReadOnly();

                // 获取并缓存用户所有权限和模块
                var _authids = orgUser.SysAuths.Select(e => e.Id);
                auths = AppPortal.Auths.Where(e => _authids.Contains(e.Id) || e.IsPublic == true).ToList().AsReadOnly();

                var _moduletype = SysAuth.TypeEnum.Module.ToString();

                var _moduleids = auths.Where(e => e.Type == _moduletype).Select(e => e.AuthObjId.ToGuid()).ToList();

                AppPortal.Modules.Where(e => _moduleids.Contains(e.Entity.Id) || e.Entity.Status == SysModule.ModuleStatus.Enabled).ToList().All((e) =>
                {
                    modules.Add(e);

                    return true;
                });
            }

            LogManager.Log("Refreshed UserSession");
        }

        #endregion

        #region Static Member

        public static UserSession Current
        {
            get
            {
                UserSession _us = null;

                if (AppPortal.CurrentUser != null)
                {
                    if (!String.IsNullOrEmpty(AppPortal.CurrentUser.SessionId))
                    {
                        _us = Get(AppPortal.CurrentUser.SessionId);
                    }
                }

                return _us;
            }
        }

        public static AppUser GetUser(string sessionId)
        {
            var _us = UserSession.Get(sessionId);

            return (_us == null ? null : _us.UserInfo);
        }

        internal static UserSession Get(string sessionId)
        {
            string xpath = GetUserSessionPath(sessionId);

            UserSession userSession = CacheManager.SystemCache.Retrieve(xpath) as UserSession;

            if (userSession != null)
            {
                userSession.UserInfo.SetLastAccessed(DateTime.Now);
            }

            return userSession;
        }

        internal static UserSession GetByUserId(string userId)
        {
            IEnumerable<UserSession> userSessions = CacheManager.SystemCache.RetrieveCascaded(GetUserSessionPathBase)
                .Select(e => e as UserSession).Where(e => e != null);

            UserSession userSession = userSessions.Where(s => s.UserInfo.UserId == userId).FirstOrDefault();

            return userSession;
        }

        internal static bool ExistsSession(string sessionId)
        {
            return Get(sessionId) != null;
        }

        internal static void Release(string sessionId)
        {
            string xpath = GetUserSessionPath(sessionId);

            CacheManager.SystemCache.Remove(xpath);
        }

        internal static void Set(UserSession userSession)
        {
            if (ExistsSession(userSession.SessionId))
            {
                Release(userSession.SessionId);
            }

            string xpath = GetUserSessionPath(userSession.SessionId);

            CacheManager.SystemCache.Set(xpath, userSession, UserSessionConfig.Instance.CachePolicy);
        }

        #region Private Methods

        private static string GetUserSessionPathBase
        {
            get
            {
                return String.Format(@"/{0}/{1}", CacheNames.System, UserSession.SessionKey);
            }
        }

        private static string GetUserSessionPath(string sessionId)
        {
            return String.Format(@"{0}/_{1}", GetUserSessionPathBase, sessionId);
        }

        private static string NewSessionId()
        {
            return SystemHelper.NewCombId().ToString();
        }

        #endregion

        #endregion
    }
}
