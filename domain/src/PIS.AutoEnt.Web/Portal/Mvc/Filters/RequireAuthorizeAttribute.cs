using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Web.Portal;

namespace PIS.AutoEnt.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequireAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var identity = HttpContext.Current.User.Identity;

            if (identity.IsAuthenticated)
            {
                WebHelper.OnWebAuthorization();

                // Check authentication
                if (AppPortal.CurrentUser == null)
                {
                    AppSecurity.Logout();

                    OnAuthFailed(filterContext);
                }
            }
            else
            {
                //未登录用户，则判断是否是匿名访问  
                var attr = filterContext.ActionDescriptor.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

                if (!isAnonymous)
                {
                    OnAuthFailed(filterContext);
                }
            }
        }

        /// <summary>
        /// 验证未成功, 则转向登录页面  
        /// </summary>
        private void OnAuthFailed(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl, true);
        }
    }
}
