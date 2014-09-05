using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.XData.DataStore
{
    public class XDataStore : IDataStore
    {
        #region Properties

        protected EasyDictionary<XDataRegion> Regions { get; private set; }

        #endregion

        #region Constructors

        public XDataStore()
        {
            Regions = new EasyDictionary<XDataRegion>();
        }

        #endregion

        #region Public Methods

        public virtual XDataRegion GetRegion(string name)
        {
            return Regions.Get(name);
        }

        public virtual void Load(XDataRegion region)
        {
            this.Regions.Set(region.Name, region);
        }

        #endregion
    }
}
