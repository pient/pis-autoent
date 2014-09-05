using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.XData
{
    public abstract class XDataObject : XObject
    {
        #region 属性

        private string _RootName = "XData";

        /// <summary>
        /// 根节点名
        /// </summary>
        public override string RootName
        {
            get { return _RootName; }
        }

        private string _RootPath = "XData";

        /// <summary>
        /// 根路径
        /// </summary>
        public virtual string RootPath
        {
            get { return _RootPath; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造空的XObject
        /// </summary>
        public XDataObject()
        {
        }

        /// <summary>
        /// 构造空的XObject
        /// </summary>
        public XDataObject(IXDataStorage store) : base(store)
        {
            this._RootName = store.Metadata.RootName;
            this._RootPath = store.Metadata.RootPath;
        }

        #endregion

        #region XObject 成员

        /// <summary>
        /// 删除符合Xpath条件的所有节点
        /// </summary>
        /// <param name="xpath"></param>
        public virtual void RemoveAll(string xpath)
        {
            ((IXDataStorage)Store).RemoveAll(xpath);
        }

        protected override string GetXPath(string xpath)
        {
            if (xpath.IndexOf('/') != 0)
            {
                xpath = "/" + xpath;
            }

            xpath = this.RootPath + xpath;

            return xpath;
        }

        protected override string GetXAttrPath(string xpath, string attrName)
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
