using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.XData
{
    public enum NodePosition
    {
        First,
        Last,
        Before,
        After
    }

    public abstract class XObject
    {
        #region 属性

        public abstract string RootName
        {
            get;
        }

        public virtual IXObjectStorage Store
        {
            get;
            protected set;
        }

        #endregion

        #region 构造函数

        public XObject(){
        }

        /// <summary>
        /// 构造空的XObject
        /// </summary>
        public XObject(IXObjectStorage store)
        {
            this.Store = store;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 由xpath获取Node
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XNode GetNode(string xpath)
        {
            XNode node = Store.GetSingleNode(xpath);

            return node;
        }

        /// <summary>
        /// 由xpath获取node列表
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XNodeList GetNodeList(string xpath)
        {
            XNodeList nodes = Store.GetNodes(xpath);

            return nodes;
        }

        /// <summary>
        /// 查看指定的节点是否存在
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public virtual bool Exists(string xpath)
        {
            xpath = this.GetXPath(xpath);

            return Store.Exists(xpath);
        }

        /// <summary>
        /// 查看指定的属性节点是否存在
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public virtual bool Exists(string xpath, string attrName)
        {
            xpath = this.GetXAttrPath(xpath, attrName);

            return Store.Exists(xpath);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public virtual string GetValue(string xpath, string attrName = null)
        {
            if (!String.IsNullOrEmpty(attrName))
            {
                xpath = this.GetXAttrPath(xpath, attrName);
            }
            else
            {
                xpath = this.GetXPath(xpath);
            }

            return Store.GetValue(xpath);
        }

        /// <summary>
        /// 获取值（泛型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public virtual T GetValue<T>(string xpath, string attrName = null)
        {
            string val = this.GetValue(xpath, attrName);

            return CLRHelper.ConvertValue<T>(val);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="value"></param>
        public virtual void SetValue(string xpath, string value)
        {
            xpath = this.GetXPath(xpath);

            Store.SetValue(xpath, value);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attrName"></param>
        /// <param name="value"></param>
        public virtual void SetValue(string xpath, string attrName, string value)
        {
            xpath = this.GetXAttrPath(xpath, attrName);

            Store.SetValue(xpath, value);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="name"></param>
        public virtual void Remove(string xpath, string attrName = null)
        {
            if (!String.IsNullOrEmpty(attrName))
            {
                xpath = this.GetXAttrPath(xpath, attrName);
            }
            else
            {
                xpath = this.GetXPath(xpath);
            }

            Store.Remove(xpath);
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="xmlsrc"></param>
        /// <param name="position"></param>
        public virtual XNode InsertEle(string xpath, string eleName, string xml, NodePosition position)
        {
            xpath = this.GetXPath(xpath);

            XNode node = Store.InsertEle(xpath, eleName, xml, position);

            return node;
        }

        /// <summary>
        /// 插入属性
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attrName"></param>
        /// <param name="value"></param>
        /// <param name="position"></param>
        public virtual XNode InsertAttr(string xpath, string attrName, string value, NodePosition position)
        {
            xpath = this.GetXPath(xpath);

            XNode node = Store.InsertAttr(xpath, attrName, value, position);

            return node;
        }

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public virtual XNode ReplaceEle(string xpath, string eleName, string value)
        {
            XNode node = this.InsertEle(xpath, eleName, value, NodePosition.After);

            this.Remove(xpath);

            return node;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public virtual T GetObject<T>(string xpath)
        {
            T rtn = default(T);

            string val = this.GetValue(xpath);

            if (val != null)
            {
                rtn = CLRHelper.DeserializeFromXmlString<T>(val);
            }

            return rtn;
        }

        /// <summary>
        /// 设置对象值
        /// </summary>
        /// <param name="xpath"></param>
        public virtual void SetObject<T>(string xpath, object obj)
        {
            string obj_xml = CLRHelper.SerializeToXmlString(obj);

            this.SetValue(xpath, obj_xml);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取Xpath, 若xpath包含反斜杠则，则xpath本身就是个xpath，否则xpath为/RootName/xpath
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        protected virtual string GetXPath(string xpath)
        {
            string t_xpath = xpath;

            if (String.IsNullOrEmpty(xpath))
            {
                t_xpath = ".";
            }
            else
            {
                t_xpath = xpath.TrimEnd('/');
            }

            //if (!String.IsNullOrEmpty(xpath) && xpath.IndexOf('/') != 0)
            //{
            //    t_xpath = "/" + t_xpath;
            //}

            //if (t_xpath.IndexOf("/" + this.RootName) != 0)
            //{
            //    t_xpath = "/" + this.RootName + t_xpath;
            //}

            return t_xpath;
        }

        /// <summary>
        /// 获取属性, 若attrName为null，则假设xpath本身就是个attribute，否则xpath为/xpath/@attrName
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        protected virtual string GetXAttrPath(string xpath, string attrName)
        {
            xpath = GetXPath(xpath);

            if (!String.IsNullOrEmpty(attrName))
            {
                xpath = xpath + "/@" + attrName;
            }

            return xpath;
        }

        #endregion
    }
}
