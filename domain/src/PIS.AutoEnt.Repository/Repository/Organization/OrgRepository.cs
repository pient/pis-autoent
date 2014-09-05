using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public abstract class OrgRepository<T> : SysMetaObjectRepository<T>, IStdObjRepository<T> 
        where T : class, ISysStdObject
    {
        #region Constructors

        protected OrgRepository()
        {
        }

        protected OrgRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion
    }
}
