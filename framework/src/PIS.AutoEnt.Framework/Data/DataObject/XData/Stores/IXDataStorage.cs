using System;
using System.Collections.Generic;
using System.Text;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt
{
    public interface IXDataStorage : IXObjectStorage, IDisposable
    {
        XDataMeta Metadata
        {
            get;
        }

        void RemoveAll(string xpath);
    }
}
