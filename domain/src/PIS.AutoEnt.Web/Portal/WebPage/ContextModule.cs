using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Principal;
using System.Web.Security;
using PIS.AutoEnt;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt.Web
{
    /// <summary>
    /// 处理用户验证等信息
    /// </summary>
    public class ContextModule : IHttpModule
    {
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);

            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
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
            // 防外部链接
            //if (!WebHelper.IsOuterLink())
            //{
            AcquireRequestIdentity();
            //}
        }

        /// <summary>
        /// 请求开始, 绑定Session(Session per request)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            // ISession session = null; // NHibernateManager.SessionFactory.OpenSession();

            // CurrentSessionContext.Bind(session);
        }

        /// <summary>
        /// 请求结束，解除Session绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_EndRequest(object sender, EventArgs e)
        {
            //if (CurrentSessionContext.HasBind(NHibernateManager.SessionFactory))
            //{
            //    var session = CurrentSessionContext.Unbind(NHibernateManager.SessionFactory);

            //    session.Dispose();
            //}
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
                string uistr = formIdentity.Ticket.UserData;

                if (!String.IsNullOrEmpty(uistr))
                {
                    UserInfo ui = CLRHelper.DeserializeFromBase64<UserInfo>(uistr);

                    try
                    {
                        SysIdentity si = new SysIdentity(ui);
                        SysPrincipal sp = new SysPrincipal(si);
                        HttpContext.Current.User = sp;
                    }
                    catch
                    {
                        PortalManager.SignOut(ui.SessionId);
                    }
                }
            }
        }

        #endregion
    }
}
