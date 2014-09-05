using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    /// <summary>
    /// 对象DataStore, 用于ORM
    /// </summary>
    public abstract class ObjDataStore : IDataStorage
    {
        public ObjDataStore()
        {
        }

        public abstract void Configure();

        public abstract void Dispose();
    }
}
