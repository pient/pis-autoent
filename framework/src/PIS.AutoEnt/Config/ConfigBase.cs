using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.Config
{
    /// <summary>
    /// 配置文件接口
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        void Reload();

        /// <summary>
        /// 配置名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        XmlNode GetConfig(string configName);
    }

    /// <summary>
    /// 配置基类
    /// </summary>
    public class ConfigBase : IConfig
    {
        #region 成员属性 

        /// <summary>
        /// 配置信息
        /// </summary>
        protected XmlNode configData;


        private string name = String.Empty;

        /// <summary>
        /// 配置名
        /// </summary>
        public virtual string Name
        {
            get { return configData.Name; }
        }


        public ConfigBase(XmlNode configData)
        {
            if (ConfigHelper.IsProtectedConfig(configData))
            {
                this.configData = ConfigHelper.DecryptConfig(configData);
            }
            else
            {
                this.configData = configData;
            }
        }

        #endregion

        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        public virtual void Reload() { }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public XmlNode GetConfig(string configName = ".")
        {
            XmlNode xmlNode = configData.SelectSingleNode(@"./" + configName);

            return xmlNode;
        }

        #region 保护方法

        /// <summary>
        /// 获取指定节下属性配置信息配置信息
        /// </summary>
        /// <param name="sectionPath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public virtual string RetrieveAttributeSetting(string sectionPath, string attributeName)
        {
            XmlNode xmlData = configData.SelectSingleNode(sectionPath);

            string setting = String.Empty;

            if (xmlData != null)
            {
                if (xmlData != null && xmlData.Attributes[attributeName] != null)
                {
                    setting = xmlData.Attributes[attributeName].Value;
                }
            }

            return setting;
        }

        /// <summary>
        /// 获取指定节下属性信息并转换为字典信息
        /// </summary>
        /// <param name="sectionPath"></param>
        /// <returns></returns>
        public virtual EasyDictionary RetrieveAttributeSettings(string sectionPath)
        {
            XmlNode xmlData = configData.SelectSingleNode(sectionPath);

            EasyDictionary settings = new EasyDictionary();

            if (xmlData != null)
            {
                foreach (XmlAttribute attr in xmlData.Attributes)
                {
                    settings.Set(attr.Name, attr.Value);
                }
            }

            return settings;
        }

        /// <summary>
        /// 获取指定节下配置信息并转换为字典信息
        /// </summary>
        /// <param name="sectionPath"></param>
        /// <returns></returns>
        public virtual EasyDictionary RetrieveNodeSettings(string sectionPath)
        {
            XmlNode xmlData = configData.SelectSingleNode(sectionPath);

            EasyDictionary settings = new EasyDictionary();

            if (xmlData != null)
            {
                foreach (XmlNode typeNode in xmlData.ChildNodes)
                {
                    settings.Set(typeNode.Name, typeNode.InnerXml);
                }
            }

            return settings;
        }

        /// <summary>
        /// 由类型属性创建对象
        /// </summary>
        /// <param name="typeAttribute"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected object CreateObjectByAttribute(string typeAttribute, params object[] args)
        {
            if (configData != null && configData.Attributes[typeAttribute] != null
                && !String.IsNullOrEmpty(configData.Attributes[typeAttribute].Value))
            {
                return Activator.CreateInstance(Type.GetType(configData.Attributes[typeAttribute].Value), args);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
