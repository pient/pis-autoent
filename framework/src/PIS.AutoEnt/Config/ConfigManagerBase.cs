using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.Config
{
    /// <summary>
    /// 配置管理器接口
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// 配置信息集合
        /// </summary>
        Dictionary<Type, IConfig> Configs { get; }
    }

    /// <summary>
    /// 配置管理基类
    /// </summary>
    public class ConfigManagerBase : IConfigManager
    {
        protected XmlNode configData;

        #region 构造函数

        /// <summary>
        /// 配置数据
        /// </summary>
        /// <param name="configData"></param>
        public ConfigManagerBase(XmlNode configData)
        {
            this.configData = configData;

            configs = new Dictionary<Type, IConfig>();
        }

        #endregion

        #region IConfigurationManager实现

        protected Dictionary<Type, IConfig> configs;

        public Dictionary<Type, IConfig> Configs
        {
            get { return configs; }
        }

        #endregion
    }
}
