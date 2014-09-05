using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public abstract class SysRepository<T> : SysMetaObjectRepository<T>
        where T : class, ISysMetaObject
    {
        #region Constructors

        protected SysRepository()
        {
        }

        public SysRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion
    }
}
