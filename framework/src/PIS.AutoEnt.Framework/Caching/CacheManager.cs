using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Caching;

namespace PIS.AutoEnt
{
    public sealed class CacheManager
    {
        #region 变量属性

        #endregion

        #region 构造函数，单体模式

        static CacheManager instance;

        internal static CacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CacheManager();
                }

                return instance;
            }
        }

        private CacheManager()
        {
        }

        #endregion

        #region Static Methods

        public static SystemCache SystemCache
        {
            get { return SystemCache.Instance; }
        }

        #endregion
    }
}
