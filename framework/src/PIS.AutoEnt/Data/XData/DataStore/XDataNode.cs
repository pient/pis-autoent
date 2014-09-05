using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.XData.DataStore
{
    [Serializable]
    public class XDataNode<T> : XDataItem<T>, IDataNode<T>
        where T : XDataItem<T>
    {
        [XmlArray]
        public EasyCollection<T> Items
        {
            get;
            set;
        }

        [XmlArray]
        public EasyCollection<XDataNode<T>> Nodes
        {
            get;
            set;
        }

        public XDataNode()
        {
            Items = new EasyCollection<T>();
            Nodes = new EasyCollection<XDataNode<T>>();
        }

        public XDataNode(XDataNode<T> parentNode)
            : base(parentNode)
        {
            Items = new EasyCollection<T>();
            Nodes = new EasyCollection<XDataNode<T>>();
        }

        #region Public Methods

        public T GetItemByCode(string code)
        {
            var item = this.Items.Where(i => i.Code == code).FirstOrDefault();

            return item;
        }

        public XDataNode<T> GetNodeByCode(string code)
        {
            var node = this.Nodes.Where(i => i.Code == code).FirstOrDefault();

            return node;
        }

        public void RemoveItemByCode(string code)
        {
            var item = this.GetItemByCode(code);

            if (item != null)
            {
                this.Items.Remove(item);
            }
        }

        public void RemoveNodeByCode(string code)
        {
            var node = this.GetNodeByCode(code);

            if (node != null)
            {
                this.Nodes.Remove(node);
            }
        }

        public void ClearItems()
        {
            this.Items.Clear();
        }

        public void ClearNodes()
        {
            this.Nodes.Clear();
        }

        public void Clear()
        {
            this.ClearItems();
            this.ClearNodes();
        }

        #endregion
    }
}
