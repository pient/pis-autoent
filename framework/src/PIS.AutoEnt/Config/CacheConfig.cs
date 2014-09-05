using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.Config
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheConfig : ConfigBase
    {
        #region 构造函数

        public CacheConfig(XmlNode sections)
            : base(sections)
        {

        }

        #endregion
    }
}
