using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt.DataContext
{
    [Serializable]
    [XmlRoot("RegTag")]
    public class RegTag : SysTag
    {
        #region Properties

        public RegSystemTag System
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public RegTag()
        {
            System = new RegSystemTag();
        }

        #endregion
    }
}
