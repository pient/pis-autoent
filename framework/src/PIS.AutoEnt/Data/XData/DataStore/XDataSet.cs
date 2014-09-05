using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PIS.AutoEnt.XData.DataStore
{
    public class XDataSet<T, TN>
        where T : XDataItem<T>
        where TN : XDataNode<T>
    {
        #region Properties

        [XmlArray]
        public EasyCollection<TN> Nodes
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public XDataSet()
        {
            Nodes = new EasyCollection<TN>();
        }

        #endregion

        #region Public Methods

        internal TN GetNodeByCode(string code)
        {
            var node = this.Nodes.Where(n => n.Code == code).FirstOrDefault();

            return node;
        }

        #endregion

        #region Support Methods

        internal XDataIndex RetrieveIdIndex()
        {
            var dataIndex = new XDataIndex(XDataRegion.IdIndexKey);
            var _nodes = this.Nodes.OrderBy(i => i.SortIndex);

            foreach (var node in _nodes)
            {
                RetrieveIdIndex(dataIndex, node);
            }

            return dataIndex;
        }

        internal XDataIndex RetrieveIdIndex(XDataIndex dataIndex, TN dataNode)
        {
            dataIndex.Set(dataNode.Id, dataNode);

            var _items = dataNode.Items.OrderBy(i => i.SortIndex);
            var _nodes = dataNode.Nodes.Select(n => n as TN).OrderBy(i => i.SortIndex);

            foreach (var item in _items)
            {
                item.ParentNode = dataNode;

                dataIndex.Set(item.Id, item);
            }

            foreach (var node in _nodes)
            {
                node.ParentNode = dataNode;

                RetrieveIdIndex(dataIndex, node);
            }

            return dataIndex;
        }

        internal XDataIndex RetrieveCodeIndex()
        {
            var dataIndex = new XDataIndex(XDataRegion.CodeIndexKey);
            var _nodes = this.Nodes.OrderBy(i => i.SortIndex);

            foreach (var node in _nodes)
            {
                RetrieveCodeIndex(dataIndex, node);
            }

            return dataIndex;
        }

        internal XDataIndex RetrieveCodeIndex(XDataIndex dataIndex, XDataNode<T> dataNode)
        {
            dataIndex.Set(dataNode.Code, dataNode);
            var _nodes = dataNode.Nodes.OrderBy(i => i.SortIndex);

            foreach (var node in _nodes)
            {
                RetrieveCodeIndex(dataIndex, node);
            }

            return dataIndex;
        }

        #endregion
    }
}
