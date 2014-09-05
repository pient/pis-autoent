using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.Config
{
    public class ExceptionConfig : ConfigBase
    {
        #region 属性

        private string exceptionPolicy = string.Empty;

        /// <summary>
        /// 默认异常处理策略
        /// </summary>
        public string ExceptionPolicy
        {
            get { return exceptionPolicy; }
        }

        #endregion

        #region 构造函数

        public ExceptionConfig(XmlNode sections)
            : base(sections)
        {
            if (configData.Attributes["ExceptionPolicy"] != null)
            {
                exceptionPolicy = configData.Attributes["ExceptionPolicy"].Value ?? String.Empty;
            }
        }

        #endregion


        #region 私有函数

        #endregion
    }
}
