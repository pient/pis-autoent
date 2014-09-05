using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.XData
{
    public class XItemList<T> : IEnumerable<T> where T : XItem 
    {
        #region 成员属性

        protected IList<T> innerNodes;

        public virtual int Count
        {
            get { return innerNodes.Count; }
        }

        public virtual T this[int i]
        {
            get { return innerNodes[i]; }
        }

        #endregion

        #region 构造函数

        public XItemList()
        {
            innerNodes = new List<T>();
        }

        public XItemList(IEnumerable<T> nodes)
        {
            this.innerNodes = nodes.ToList();
        }

        #endregion

        #region 公共成员

        public virtual T Get(int i)
        {
            return innerNodes[i];
        }

        #endregion

        #region IEnumerable成员

        public virtual IEnumerator<T> GetEnumerator()
        {
            return innerNodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerNodes.GetEnumerator();
        }

        #endregion
    }
}
