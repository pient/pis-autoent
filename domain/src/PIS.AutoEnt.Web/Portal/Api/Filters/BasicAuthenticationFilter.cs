using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Net;
using System.Web.Security;
using PIS.AutoEnt.Security;
using System.Web;

namespace PIS.AutoEnt.Web.Api
{
    public class BasicAuthenticationFilter : ActionFilterAttribute
    {
        #region Consts

        public const string WEBAPI_AUTH_FLAG = "WebApiAuthenticatedFlag";

        #endregion

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //检验用户ticket信息，用户ticket信息来自调用发起方  
            if (actionContext.Request.Headers.Authorization != null)
            {
                //解密用户ticket,并校验用户名密码是否匹配  
                var encryptTicket = actionContext.Request.Headers.Authorization.Parameter;

                if (ValidateUserTicket(encryptTicket))
                {
                    base.OnActionExecuting(actionContext);
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                //检查web.config配置是否要求权限校验
                bool isRquired = false;

                if (WebConfigurationManager.AppSettings[WEBAPI_AUTH_FLAG] != null)
                {
                    isRquired = WebConfigurationManager.AppSettings[WEBAPI_AUTH_FLAG].ToString() == "true";
                }

                if (isRquired)
                {
                    //如果请求Header不包含ticket，则判断是否是匿名调用  
                    var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                    bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

                    //是匿名用户，则继续执行；非匿名用户，抛出“未授权访问”信息  
                    if (isAnonymous)
                        base.OnActionExecuting(actionContext);
                    else
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else
                {
                    base.OnActionExecuting(actionContext);
                }
            }
        }

        /// <summary>  
        /// 校验用户ticket信息  
        /// </summary>  
        /// <param name="encryptTicket"></param>  
        /// <returns></returns>  
        private bool ValidateUserTicket(string encryptTicket)
        {
            var userTicket = FormsAuthentication.Decrypt(encryptTicket);
            var userData = userTicket.UserData;

            AppUser appUser = CLRHelper.DeserializeFromBase64<AppUser>(userData);

            if (appUser != null)
            {
                HttpContext.Current.User = new SysPrincipal(appUser);
            }

            return false;
        } 
    }
}
