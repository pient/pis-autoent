using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Config
{
    public sealed class ConfigSections
    {
        #region Consts

        public const string Log = "Log"; // 日志配置段名

        public const string Log4net = "Log4net";   // Logs/Log4net配置段

        public const string Cache = "Caching";  // 缓存配置段名

        public const string Service = "Services";  // 服务配置段名

        public const string ThirdParty = "ThirdParties"; // 第三方软件配置段名

        public const string Exception = "Exception"; // 异常配置段名

        #endregion
    }
}
