using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using PIS.AutoEnt.XData;
using PIS.AutoEnt.Data.Query;

namespace PIS.AutoEnt.Data
{
    public abstract class DataRepository : IDataRepository
    {
        #region 成员变量

        public IEntityContext EntityContext
        {
            get;
            protected set;
        }

        /// <summary>
        /// 对象数据存储
        /// </summary>
        public abstract ObjDataStore ObjDataStore
        {
            get;
        }

        /// <summary>
        /// 数据存储
        /// </summary>
        public abstract DbDataStore DbDataStore
        {
            get;
        }

        public IQueryBuilder QueryBuilder
        {
            get;
            protected set;
        }

        #endregion

        #region 构造函数

        public DataRepository(IEntityContext ctx)
        {
            this.EntityContext = ctx;
        }

        #endregion

        #region IDataRepository成员

        public virtual void Dispose()
        {
            this.EntityContext.SaveChanges();

            this.EntityContext.Dispose();
        }

        #endregion
    }

    public abstract class DataRepository<T> : DataRepository, IDataRepository<T> where T : class
    {
        #region 构造函数

        public DataRepository(IEntityContext ctx)
            : base(ctx)
        {
        }

        #endregion

        #region IDataRepository成员

        public abstract T Create(T entity);

        public abstract T Find(params object[] keyValues);
        public abstract T Find(Expression<Func<T, bool>> predicate);
        public abstract IList<T> FindAll(Expression<Func<T, bool>> predicate);

        public abstract void Delete(T entity);

        public abstract bool Update(T entity);

        public override void Dispose()
        {
        }

        #endregion

    }
}
