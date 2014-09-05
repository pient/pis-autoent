using System;
using System.Threading;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using PIS.AutoEnt;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Web.Security;

namespace PIS.AutoEnt.Web.Security
{
    /// <summary>
    /// 处理用户的验证
    /// </summary>
    public class AuthModule : IHttpModule
    {
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        public void Dispose()
        {
        }

        #endregion

        /// <summary>
        /// 认证请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            AcquireRequestIdentity();
        }

        #region 静态方法

        /// <summary>
        /// 获取登陆标识信息
        /// </summary>
        internal static void AcquireRequestIdentity()
        {
            IPrincipal user = HttpContext.Current.User;
            string requestPath = HttpContext.Current.Request.FilePath.ToLower();

            // 只有aspx页面才需要验证
            if (!(requestPath.EndsWith("aspx") || requestPath.EndsWith("ashx")))
            {
                return;
            }

            // 用户认证
            if (user != null && user.Identity.IsAuthenticated && user.Identity.AuthenticationType == "Forms")
            {
                FormsIdentity formIdentity = user.Identity as FormsIdentity;

                if (formIdentity.Ticket != null)
                {
                    try
                    {
                        SysPrincipal sp = new SysPrincipal(new SysFormsIdentity(formIdentity.Ticket));

                        HttpContext.Current.User = sp;
                    }
                    catch
                    {
                        AppSecurity.Logout();
                    }
                }
            }

            // RewriteRequestPath();
        }

        /// <summary>
        /// rewrite request
        /// </summary>
        internal static void RewriteRequestPath()
        {
            string requestPath = HttpContext.Current.Request.FilePath;

            HttpContext.Current.RewritePath(requestPath);
        }

        #endregion
    }

}
