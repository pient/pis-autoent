using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.XData
{
    public class XDataNode : XDataObject
    {
        #region 构造函数

        /// <summary>
        /// 构造空的XObject
        /// </summary>
        public XDataNode()
        {
        }

        /// <summary>
        /// 构造空的XObject
        /// </summary>
        public XDataNode(IXDataStorage store)
            : base(store)
        {
            
        }

        #endregion
    }
}
