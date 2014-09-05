using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace PIS.AutoEnt.Config
{
    public sealed class ConfigManager : ConfigManagerBase
    {
        #region 成员

        public const string SourceSectionName = @"Source";
        public const string TopSectionName = @"PIS";
        public const string SysSectionPath = @"//System";    // 系统配置节路径

        private SystemConfig systemConfig;

        #endregion

        #region 属性

        private static ConfigManager instance;

        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = System.Configuration.ConfigurationManager.GetSection(TopSectionName) as ConfigManager;
                }

                return instance;
            }
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        public static SystemConfig SystemConfig
        {
            get
            {
                return Instance.systemConfig;
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static Dictionary<string, string> ConnectionStrings
        {
            get
            {
                return SystemConfig.ConnectionStrings;
            }
        }

        /// <summary>
        /// 程序定义
        /// </summary>
        public static Dictionary<string, string> AppSettings
        {
            get { return SystemConfig.AppSettings; }
        }

        /// <summary>
        /// 日志配置
        /// </summary>
        public static LogConfig LogConfig
        {
            get
            {
                return Instance.GetConfig<LogConfig>(ConfigSections.Log);
            }
        }

        /// <summary>
        /// 缓存配置
        /// </summary>
        public static CacheConfig CacheConfig
        {
            get
            {
                return Instance.GetConfig<CacheConfig>(ConfigSections.Cache);
            }
        }

        /// <summary>
        /// 服务配置
        /// </summary>
        public static ServiceConfig ServiceConfig
        {
            get
            {
                return Instance.GetConfig<ServiceConfig>(ConfigSections.Service);
            }
        }

        /// <summary>
        /// 第三方配置
        /// </summary>
        public static ThirdPartyConfig ThirdPartyConfig
        {
            get
            {
                return Instance.GetConfig<ThirdPartyConfig>(ConfigSections.ThirdParty);
            }
        }

        /// <summary>
        /// 异常配置
        /// </summary>
        public static ExceptionConfig ExceptionConfig
        {
            get
            {
                return Instance.GetConfig<ExceptionConfig>(ConfigSections.Exception);
            }
        }

        #endregion

        #region 构造函数

        internal ConfigManager(XmlNode sections)
            : base(sections)
        {
            Initialize();
        }

        #endregion

        #region IConfiguration实现

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            if (configData != null)
            {
                XmlNode sysConfigData = configData.SelectSingleNode(SysSectionPath);

                if (sysConfigData.Attributes[SourceSectionName] != null)
                {
                    string source = sysConfigData.Attributes[SourceSectionName].Value;

                    if (!String.IsNullOrEmpty(source))
                    {
                        source = SystemHelper.GetPath(source);

                        if (File.Exists(source))
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(source);

                            sysConfigData = xmlDoc.DocumentElement;
                        }
                    }
                }

                systemConfig = new SystemConfig(sysConfigData);
            }
        }

        /// <summary>
        /// 重新从本地加载文件
        /// </summary>
        public void Reload()
        {
            try
            {
                ReloadConfig();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 刷新配置文件
        /// </summary>
        private void ReloadConfig()
        {
            SystemConfig.Reload();
        }

        private T GetConfig<T>(string sectionName) where T : class, IConfig
        {
            return SystemConfig.Configs[sectionName] as T;
        }

        #endregion
    }
}
