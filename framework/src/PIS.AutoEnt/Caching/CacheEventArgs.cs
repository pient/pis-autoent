using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Caching
{
    /// <summary>
    /// 缓存事件参数
    /// </summary>
    public class CacheEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// 被操作的缓存
        /// </summary>
        public CacheNode CacheNode { get; protected set; }

        /// <summary>
        /// 缓存配置项
        /// </summary>
        public object Config { get; protected set; }

        #endregion

        #region Constructors

        public CacheEventArgs(CacheNode cacheNode, object config)
        {
            this.CacheNode = cacheNode;
            this.Config = config;
        }

        #endregion
    }

    /// <summary>
    /// 缓存移除事件参数
    /// </summary>
    public class CacheRemoveEventArgs : CacheEventArgs
    {
        #region Properties

        /// <summary>
        /// 是否级联操作
        /// </summary>
        public bool IsCascaded
        {
            get;
            protected set;
        }

        #endregion

        #region Constructors

        public CacheRemoveEventArgs(CacheNode cacheNode, bool isCascaded = false)
            : base(cacheNode, null)
        {
            this.IsCascaded = isCascaded;
        }

        #endregion
    }
}
