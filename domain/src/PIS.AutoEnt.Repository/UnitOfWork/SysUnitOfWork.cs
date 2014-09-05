using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Pattern;

namespace PIS.AutoEnt.Repository
{
    public class SysUnitOfWork : UnitOfWork<SysDbContext>, ISysUnitOfWork
    {
    }
}
