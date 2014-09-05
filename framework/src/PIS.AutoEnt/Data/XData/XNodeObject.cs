using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.Data.XData
{
    public abstract class XNodeObject : XObject
    {
        #region XDataObject成员

        /// <summary>
        /// 根节点名
        /// </summary>
        public override string RootName
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 节点名，除根节点，默认Node
        /// </summary>
        public virtual string NodeName
        {
            get { return "node"; }
        }

        /// <summary>
        /// 节点集名，默认Nodes
        /// </summary>
        public virtual string CollectionNodeName
        {
            get { return "nodes"; }
        }

        #endregion

        #region 构造函数

        public XNodeObject()
        {
        }

        #endregion
    }
}
