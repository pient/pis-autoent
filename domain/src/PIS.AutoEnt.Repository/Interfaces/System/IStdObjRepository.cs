using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Repository.Interfaces
{
    public interface IStdObjRepository<T> : IMetaObjectRepository<T> 
        where T: class, ISysStdObject
    {
    }
}
