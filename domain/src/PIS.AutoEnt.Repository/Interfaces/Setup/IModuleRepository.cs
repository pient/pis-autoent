using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Repository.Interfaces
{
    public interface IModuleRepository : IStdObjRepository<SysModule>, IStructedRepository<SysModule>
    {
        IAuthRepository AuthRepository { get; }
    }
}
