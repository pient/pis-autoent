using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Repository.Interfaces
{
    public interface IRepositoryProvider
    {
        bool IsUnique(IEntityObject modelObj, Type modelType);
    }
}
