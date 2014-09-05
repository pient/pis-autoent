using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    public interface IDataNode<T> : IDataItem
        where T : IDataItem
    {
        EasyCollection<T> Items
        {
            get;
        }
    }
}
