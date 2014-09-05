using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.Config
{
    public class SystemConfig : ConfigBase
    {
        #region 属性

        private readonly Dictionary<string, IConfig> configs = new Dictionary<string, IConfig>();

        /// <summary>
        /// 配置列表
        /// </summary>
        public Dictionary<string, IConfig> Configs
        {
            get { return configs; }
        }

        private Dictionary<string, string> connectionStrings = new Dictionary<string, string>();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public Dictionary<string, string> ConnectionStrings
        {
            get { return connectionStrings; }
        }

        private Dictionary<string, string> appSettings = new Dictionary<string, string>();

        /// <summary>
        /// 程序定义
        /// </summary>
        public Dictionary<string, string> AppSettings
        {
            get { return appSettings; }
        }

        #endregion

        #region 构造函数

        internal SystemConfig(XmlNode configData)
            : base(configData)
        {
            Initialize();
        }

        #endregion

        #region BaseConfig实现

        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        public override void Reload()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public void Initialize()
        {
            connectionStrings = new Dictionary<string, string>();
            appSettings = new Dictionary<string, string>();

            EasyDictionary tmp_dict = RetrieveNodeSettings(@"//ConnectionStrings"); // 获取数据库连接字符串列表

            foreach (string key in tmp_dict.Keys)
            {
                connectionStrings.Add(key, tmp_dict.Get<string>(key));
            }

            tmp_dict = RetrieveNodeSettings(@"//AppSettings"); // 获取自定义配置信息
            foreach (string key in tmp_dict.Keys)
            {
                appSettings.Add(key, tmp_dict.Get<string>(key));
            }

            InitConfigInfo(); // 初始化配置文件信息
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 获取配置信息列表(配置管理器，和配置)
        /// </summary>
        private void InitConfigInfo()
        {
            XmlNode xmlData = configData.SelectSingleNode(@"//Configs");
            xmlData = (xmlData == null ? configData.SelectSingleNode(@"//Configurations") : xmlData);    // 兼容之前版本

            XmlNode sections = xmlData.SelectSingleNode(@"//ConfigSection");
            sections = (sections == null ? configData.SelectSingleNode(@"//ConfigurationSection") : sections);    // 兼容之前版本

            XmlNode datas = xmlData.SelectSingleNode(@"//ConfigData");
            datas = (datas == null ? configData.SelectSingleNode(@"//ConfigurationData") : datas);    // 兼容之前版本

            if (sections != null)
            {
                foreach (XmlNode typeNode in sections.SelectNodes(@"./Section"))
                {
                    string typestr = ((typeNode.Attributes["Type"] == null) ? String.Empty : typeNode.Attributes["Type"].Value);
                    string secname = ((typeNode.Attributes["Name"] == null) ? String.Empty : typeNode.Attributes["Name"].Value);

                    if (!String.IsNullOrEmpty(typestr) || !String.IsNullOrEmpty(secname))
                    {
                        object[] parameters = { datas.SelectSingleNode(@"//" + secname) }; // 类型参宿信息

                        try
                        {
                            if (configs.ContainsKey(secname))
                            {
                                configs.Remove(secname);
                            }

                            configs.Add(secname, (IConfig)Activator.CreateInstance(Type.GetType(typestr), parameters));
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
