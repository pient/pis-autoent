using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PIS.AutoEnt.XData.DataStore;

namespace PIS.AutoEnt.App
{
    public class RegDataNode : XDataNode<RegDataItem>, IRegDataObject
    {
        #region Properties

        [XmlAttribute]
        public string RegDataType { get; set; }

        [XmlAttribute]
        public string Editable { get; set; }

        [XmlElement]
        public string EditPage { get; set; }

        [XmlElement]
        public string Description { get; set; }

        #endregion

        #region Constructors

        public RegDataNode()
        {
        }

        #endregion
    }
}
