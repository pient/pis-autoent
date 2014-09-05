using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.XData
{
    public abstract class XDataRepositoryBase : IXDataRepository
    {
        #region 成员属性

        protected IXDataStorage store;

        public abstract string TableName
        {
            get;
        }

        /// <summary>
        /// XData字段名,默认XData
        /// </summary>
        public virtual string XFiledName
        {
            get { return "XData"; }
        }

        /// <summary>
        /// XPath路径
        /// </summary>
        public abstract string XPath
        {
            get;
        }

        #endregion

        #region 构造函数

        protected XDataRepositoryBase()
        {
        }

        public XDataRepositoryBase(IXDataStorage store)
        {
            this.store = store;
        }

        #endregion

        #region IDataRepository成员

        public void Delete(string xpath)
        {
            throw new NotImplementedException();
        }

        public void Insert(string xpath)
        {
            throw new NotImplementedException();
        }

        public void Update(string xpath)
        {
            throw new NotImplementedException();
        }

        public abstract string Query(string xquery, string condition = "", string orderby = "");

        public XNode QueryNode(string xquery, string condition = "", string orderby = "")
        {
            string xml_result = this.Query(xquery, condition, orderby);

            if (String.IsNullOrEmpty(xml_result))
            {
                return null;
            }

            return new XNode();
        }

        public XNodeList QueryNodes(string xquery, string condition = "", string orderby = "")
        {
            string xml_result = this.Query(xquery, condition, orderby);

            if (String.IsNullOrEmpty(xml_result))
            {
                return null;
            }

            return new XNodeList();
        }

        #endregion
    }
}
