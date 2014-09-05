using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Configuration;
using Castle.Core.Resource;
using PIS.AutoEnt.Config;

namespace PIS.AutoEnt.Pattern
{
    /// <summary>
    /// 用于获取Windsor配置文件
    /// </summary>
    public class FrameworkConfigResource : AbstractResource
    {
        private readonly XmlNode configSectionNode;

        public FrameworkConfigResource()
        {
            XmlNode node = (XmlNode)ConfigManager.ThirdPartyConfig.GetConfig(SysConsts.WindsorSectionName);

            if (node == null)
            {
                String message = String.Format(CultureInfo.InvariantCulture,
                    "Could not find section '{0}' in the configuration file associated with this domain.", SysConsts.WindsorSectionName);

                throw new ConfigurationErrorsException(message);
            }

            // TODO: Check whether it's CData section
            configSectionNode = node;
        }

        public FrameworkConfigResource(XmlNode node)
        {
            this.configSectionNode = node;
        }

        public override TextReader GetStreamReader()
        {
            return new StringReader(configSectionNode.OuterXml);
        }

        public override TextReader GetStreamReader(Encoding encoding)
        {
            throw new NotSupportedException("Encoding is not supported");
        }

        public override IResource CreateRelative(String relativePath)
        {
            throw new NotSupportedException("Relative path is not supported");
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "ConfigResource: [{0}]", SysConsts.WindsorSectionName);
        }
    }
}
