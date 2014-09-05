using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace PIS.AutoEnt.Data
{
    /// <summary>
    /// 数据仓储接口
    /// </summary>
    public interface IDataRepository : IDisposable
    {
    }

    /// <summary>
    /// 数据仓储接口（范型）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataRepository<T> :IDataRepository  where T : class
    {
        /// <summary>
        /// 实体上下文
        /// </summary>
        IEntityContext EntityContext { get; }

        T Create(T entity);

        T Find(params object[] keyValues);
        T Find(Expression<Func<T, bool>> predicate);
        IList<T> FindAll(Expression<Func<T, bool>> predicate = null);

        bool Update(T entity);

        void Delete(T entity);
    }
}
