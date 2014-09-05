using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Repository.Interfaces
{
    public interface IObjRepository<T> : IStdObjRepository<T> where T : SysObject
    {
    }

    public interface IObjRepository : IObjRepository<SysObject>
    {
    }
}
