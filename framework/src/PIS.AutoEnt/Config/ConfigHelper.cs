using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt.Config
{
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 加密配置数据
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static XmlNode EncryptConfig(XmlNode configData, string code)
        {
            string configstr = configData.OuterXml;

            configstr = CryptoManager.GetEncryptStringByMAC(configstr, code);

            configstr = "<System IsProtected=\"true\">" + configstr + "</System>";

            XmlDocument xmlDoc = configstr.LoadAsXmlDocument();

            if (xmlDoc == null)
            {
                return null;
            }

            return xmlDoc.DocumentElement;
        }

        /// <summary>
        /// 由本地信息揭秘数据
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static XmlNode DecryptConfig(XmlNode configData)
        {
            string maccode = SystemHelper.GetMACAddress();

            return DecryptConfig(configData, maccode);
        }

        /// <summary>
        /// 由本地信息揭秘数据(此函数不应暴露，这里临时使用)
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static XmlNode DecryptConfig(XmlNode configData, string maccode)
        {
            string configstr = configData.InnerXml;

            configstr = CryptoManager.GetDecryptStringByMAC(configstr, maccode);
            XmlDocument xmlDoc = configstr.LoadAsXmlDocument();

            if (xmlDoc == null)
            {
                return null;
            }

            return xmlDoc.DocumentElement;
        }

        /// <summary>
        /// 判断是否加密数据
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static bool IsProtectedConfig(XmlNode configData)
        {
            bool isPortected = false;

            // 配置数据被加密, 先进行解密
            if (configData.Attributes["IsProtected"] != null
                && String.Compare(configData.Attributes["IsProtected"].Value, "true", true) == 0)
            {
                isPortected = true;
            }

            return isPortected;
        }
    }
}
