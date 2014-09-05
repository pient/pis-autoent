using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public abstract class SysDataRepository : DataRepository, ISysRepository
    {
        #region Properties

        public SysDbContext DbContext
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        protected SysDataRepository()
            : this(new SysDbContext())
        {
        }

        protected SysDataRepository(SysDbContext ctx)
            : base(ctx)
        {
            this.DbContext = ctx;
        }

        #endregion

        #region DataRepository Members

        public override ObjDataStore ObjDataStore
        {
            get { throw new NotImplementedException(); }
        }

        public override DbDataStore DbDataStore
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }

    public abstract class SysDataRepository<T> : SysDataRepository, IDataRepository<T>
        where T : class, IEntityObject
    {
        #region Constructors

        protected SysDataRepository()
        {
        }

        protected SysDataRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion

        #region DataRepository Members

        public virtual T Create(T entity)
        {
            EntityContext.Register(entity, EntityObjectState.Added);

            return entity;
        }

        public virtual T Find(params object[] keyValues)
        {
            return DbContext.Set<T>().Find(keyValues);
        }

        public virtual T Find(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate).FirstOrDefault();
        }

        public virtual IList<T> FindAll(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return DbContext.Set<T>().ToList();
            }
            else
            {
                return DbContext.Set<T>().Where(predicate).ToList();
            }
        }

        public virtual bool Update(T entity)
        {
            EntityContext.Register(entity, EntityObjectState.Modified);

            return true;
        }

        public virtual void Delete(T entity)
        {
            EntityContext.Register(entity, EntityObjectState.Deleted);
        }

        public override void Dispose()
        {
            base.Dispose();

            DbContext.Dispose();
        }

        #endregion
    }
}
