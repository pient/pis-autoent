using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.XData.DataStore;

namespace PIS.AutoEnt.Data
{
    public interface IDataStore
    {
        XDataRegion GetRegion(string name);
    }
}
