using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using System.Xml;

namespace PIS.AutoEnt.Caching
{
    public class MemoryCacheProvider : ICacheProvider
    {
        #region Members

        protected MemoryCache innerCache;

        #endregion

        #region Constructors

        public MemoryCacheProvider(string name, XmlNode config = null)
        {
            innerCache = new MemoryCache(name, null);
        }

        #endregion

        #region ICacheProvider Members

        public bool Add(string key, object obj, object config = null)
        {
            CacheItem cacheItem = new CacheItem(key, obj);

            CacheItemPolicy policy = GetPolicy(config);

            return innerCache.Add(cacheItem, policy);
        }

        public object Retrieve(string key)
        {
            return innerCache.Get(key);
        }

        public object Remove(string key)
        {
            return innerCache.Remove(key);
        }

        #endregion

        #region Methods

        private CacheItemPolicy GetPolicy(object config)
        {
            CacheItemPolicy policy = config as CacheItemPolicy;

            if (config == null)
            {
                policy = new CacheItemPolicy();
            }

            return policy;
        }

        #endregion
    }
}
