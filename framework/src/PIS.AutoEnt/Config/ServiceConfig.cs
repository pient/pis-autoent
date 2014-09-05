using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.Config
{
    public class ServiceConfig : ConfigBase
    {
        #region 属性

        // 普通服务地址
        public string CommonServicePath
        {
            get
            {
                XmlNode commonServicePathNode = configData.SelectSingleNode(@"./CommonService/ServicesPath");
                if (commonServicePathNode == null)
                {
                    return String.Empty;
                }
                else
                {
                    return commonServicePathNode.InnerText;
                }
            }
        }

        public EasyDictionary UserSession
        {
            get
            {
                return this.RetrieveAttributeSettings(@"./UserSession");
            }
        }

        #endregion

        #region 构造函数

        public ServiceConfig(XmlNode sections)
            : base(sections)
        {
        }

        #endregion
    }
}
