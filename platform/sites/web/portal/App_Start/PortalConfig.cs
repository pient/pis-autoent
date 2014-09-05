using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIS.AutoEnt.Portal.WebSite
{
    public class PortalConfig
    {
        public static void Register()
        {
            Repository.RepoInitializer.Initialize();

            // 初始化系统
            App.ModuleInitializer.Initialize();

            // Register FormData
            FormDataMapperConfig.Register();
        }
    }
}