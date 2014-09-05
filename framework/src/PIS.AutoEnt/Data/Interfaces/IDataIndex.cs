using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    public interface IDataIndex
    {
        void Set(object key, object data);

        object Get(object key);
    }
}
