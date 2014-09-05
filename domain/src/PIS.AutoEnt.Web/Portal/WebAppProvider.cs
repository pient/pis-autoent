using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using PIS.AutoEnt.App.Interfaces;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Web.Security;
using PIS.AutoEnt.Web.Portal;

namespace PIS.AutoEnt.Web
{
    public class WebAppProvider : IAppProvider
    {
        #region IAppProvider Members

        public void SetLanguage(string lang)
        {
            HttpCookie langCookie = new HttpCookie(SysConsts.LanguageKey, lang);
            langCookie.Expires = DateTime.Now.AddYears(100);    // Language never expire

            HttpContext.Current.Response.Cookies.Set(langCookie);
        }

        /// <summary>
        /// 获取用户语言
        /// </summary>
        /// <returns></returns>
        public string GetLanguage()
        {
            String lang = String.Empty;

            HttpCookie langCookie = HttpContext.Current.Request.Cookies.Get(SysConsts.LanguageKey);

            if (langCookie != null)
            {
                lang = langCookie.Value;
            }

            return lang;
        }

        public void Logon(AppUser appUser, DtoModels.AuthInfo authInfo)
        {
            var userData = new UserData(appUser, authInfo);

            string userDataString = userData.ToJsonString();

            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(appUser.LoginName, false);
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                ticket.Version, ticket.Name, ticket.IssueDate,
                ticket.Expiration, ticket.IsPersistent, userDataString);

            authCookie.Value = FormsAuthentication.Encrypt(newTicket);

            HttpContext.Current.Response.Cookies.Add(authCookie);

            // 设置Principal，非常重要
            var _us = new SysPrincipal(appUser);
            HttpContext.Current.User = _us;
        }

        public void Logoff()
        {
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();
        }

        #endregion
    }
}
