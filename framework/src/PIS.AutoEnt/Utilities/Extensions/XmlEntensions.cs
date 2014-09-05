using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace PIS.AutoEnt
{
    public static class XmlEntensions
    {
        /// <summary>
        /// 从一个XML节点拷贝所有属性到另一个XML节点
        /// </summary>
        /// <param name="fromNode"></param>
        /// <param name="toNode"></param>
        public static void CopyAttributesTo(this XmlElement fromNode, XmlElement toNode)
        {
            XmlAttributeCollection atrs = fromNode.Attributes;

            for (int i = 0; i < atrs.Count; i++)
            {
                toNode.SetAttribute(atrs.Item(i).Name, atrs.Item(i).Value);
            }
        }

        /// <summary>
        /// 由Json字符串转换为XmlDocument
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static XmlDocument FromJsonToXml(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json);
        }

        /// <summary>
        /// 由xml字符串转换为Json字符串
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string FromXmlToJson(this string xml)
        {
            XmlDocument doc = xml.LoadAsXmlDocument();

            return ToJsonString(doc);
        }

        /// <summary>
        /// 有XmlNode转换为Json字符串
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string ToJsonString(this XmlNode xmlNode)
        {
            return Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlNode);
        }

        /// <summary>
        /// 从字符串加载Xml为XElement
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static XmlDocument LoadAsXmlDocument(this string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            return xmlDoc;
        }

        /// <summary>
        /// 获取格式化xml，默认编码utf8
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static string GetFormatXml(this string xmlstr)
        {
            string result = xmlstr;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);

            result = xmlDoc.GetFormatXml();

            return result;
        }

        /// <summary>
        /// 获格式化xml
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string GetFormatXml(this XmlNode xmlNode)
        {
            return xmlNode.GetFormatXml(Encoding.UTF8);
        }

        /// <summary>
        /// 获格式化xml
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string GetFormatXml(this XmlNode xmlNode, Encoding encoding)
        {
            string result = null;

            if (xmlNode != null)
            {
                try
                {
                    MemoryStream mstream = new MemoryStream();
                    XmlTextWriter writer = new XmlTextWriter(mstream, null);
                    writer.Formatting = Formatting.Indented;

                    xmlNode.WriteTo(writer);
                    writer.Flush();
                    writer.Close();

                    result = encoding.GetString(mstream.ToArray());
                    mstream.Close();
                }
                catch
                {
                    // Only return 
                    result = xmlNode.OuterXml.Replace(">/r/n", ">").Replace(">", ">/r/n");
                }
            }

            return result;
        }
    }
}
