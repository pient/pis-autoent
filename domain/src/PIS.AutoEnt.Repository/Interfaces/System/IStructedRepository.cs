using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Repository.Interfaces
{
    public interface IStructedRepository<T> : ISysRepository, IMetaObjectRepository<T>
        where T : class, ISysStructedObject
    {
        IDataStructureRepository StructRepository { get; }
    }
}
