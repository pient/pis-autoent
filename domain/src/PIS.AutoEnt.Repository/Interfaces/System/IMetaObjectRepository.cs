using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Repository.Interfaces
{
    public interface IMetaObjectRepository<T> : IDataRepository<T>, ISysRepository
        where T : class, ISysMetaObject
    {
    }
}
