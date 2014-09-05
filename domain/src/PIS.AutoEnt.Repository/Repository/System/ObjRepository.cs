using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public class ObjRepository<T> : SysMetaObjectRepository<T>, IObjRepository<T> 
        where T : SysObject
    {
        #region Constructors

        public ObjRepository()
        {
        }

        public ObjRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion

        #region Methods

        public T FindByCode(string code)
        {
            return DbContext.Set<T>().Where(e => e.Code == code).FirstOrDefault();
        }

        #endregion
    }

    public class ObjRepository : ObjRepository<SysObject>, IObjRepository
    {
        #region Constructors

        public ObjRepository()
        {
        }

        public ObjRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion
    }
}
