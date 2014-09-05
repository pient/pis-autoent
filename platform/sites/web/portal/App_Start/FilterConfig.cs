using System.Web;
using System.Web.Mvc;
using PIS.AutoEnt.Web;
using PIS.AutoEnt.Web.Mvc;

namespace PIS.AutoEnt.Portal.WebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionFilter());
        }
    }
}