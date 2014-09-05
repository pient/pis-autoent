using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PIS.AutoEnt.Web.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RequireAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

            if (!isAnonymous)
            {
                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else
                {
                    WebHelper.OnWebAuthorization();
                }
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
