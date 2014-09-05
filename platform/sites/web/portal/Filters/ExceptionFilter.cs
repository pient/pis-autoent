using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace PIS.AutoEnt.Portal.WebSite.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            LogManager.Log(exception.Message);
            LogManager.Log(exception.StackTrace);

            if (exception.InnerException != null)
            {
                LogManager.Log(exception.InnerException.Message);
                LogManager.Log(exception.InnerException.StackTrace);
            }

            LogManager.Log("URL: " + actionExecutedContext.Request.RequestUri.AbsoluteUri);
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            
            base.OnException(actionExecutedContext);
        }
    }
}