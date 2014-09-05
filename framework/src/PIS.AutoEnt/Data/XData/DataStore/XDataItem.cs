using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.XData.DataStore
{
    public class XDataItem : IDataItem
    {
        [XmlAttribute]
        [TypeConverter(typeof(GuidConverter))]
        public Guid Id { get; set; }

        [XmlAttribute]
        public string Code { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int SortIndex { get; set; }

        [XmlElement]
        public string DisplayData { get; set; }

        [XmlAttribute]
        public string DataType { get; set; }

        [XmlElement]
        public object Data { get; set; }

        [XmlAttribute]
        public string Status { get; set; }

        [XmlElement]
        public object Tag { get; set; }

        public XDataItem()
        {
            Id = SystemHelper.NewCombId();
        }
    }

    public class XDataItem<T> : XDataItem
        where T : XDataItem<T>
    {

        [XmlIgnore]
        public XDataNode<T> ParentNode { get; internal set; }

        public XDataItem()
        {
        }

        public XDataItem(XDataNode<T> parentNode)
            : this()
        {
            this.ParentNode = parentNode;
        }
    }
}
