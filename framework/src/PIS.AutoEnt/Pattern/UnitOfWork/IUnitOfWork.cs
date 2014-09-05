using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// Unit of Work模式接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 提交任务
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// 实体上下文
        /// </summary>
        IEntityContext Context { get; }
    }
}
