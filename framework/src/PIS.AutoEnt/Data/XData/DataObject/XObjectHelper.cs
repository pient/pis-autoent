using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt
{
    public static class XObjectHelper
    {
        #region 静态方法

        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetNodeValue(XmlNode node)
        {
            string val = node.Value;

            if (val == null)
            {
                if (node.InnerXml != null)
                {
                    val = node.InnerXml;
                }
            }

            return val;
        }

        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T GetNodeValue<T>(XmlNode node)
        {
            string value = GetNodeValue(node);

            T val = CLRHelper.ConvertValue<T>(value);

            return val;
        }

        /// <summary>
        /// 设置节点值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        public static void SetNodeValue(XmlNode node, object value)
        {
            if (node is XmlAttribute)
            {
                node.Value = value.ToString();
            }
            else
            {
                node.InnerXml = value.ToString();
            }
        }

        #endregion
    }
}
