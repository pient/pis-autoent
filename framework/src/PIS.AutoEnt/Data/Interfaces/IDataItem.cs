using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    public interface IDataItem
    {
        Guid Id { get; set; }

        int SortIndex { get; set; }
    }
}
