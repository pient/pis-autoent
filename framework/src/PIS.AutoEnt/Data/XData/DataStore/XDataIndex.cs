using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.XData.DataStore
{
    internal class XDataIndex : IDataIndex 
    {
        #region Properties

        public string Name { get; private set; }

        public EasyDictionary<XDataItem> Items { 
            get; 
            set; 
        }

        #endregion

        #region Constructors

        public XDataIndex(string name)
        {
            this.Name = name;
            Items = new EasyDictionary<XDataItem>();
        }

        #endregion

        #region IXDataIndex　Members

        public void Set(object key, object data)
        {
            this.Items.Set(key, data);
        }

        public object Get(object key)
        {
            return this.Items.Get(key);
        }

        #endregion
    }
}
