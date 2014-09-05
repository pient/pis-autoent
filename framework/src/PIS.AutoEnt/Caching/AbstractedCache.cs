using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt.Caching
{
    public abstract class AbstractedCache
    {
        #region Events

        /// <summary>
        /// event triggerred when item is set
        /// </summary>
        public event EventHandler<CacheEventArgs> OnSet;

        /// <summary>
        /// event triggerred when item is removed
        /// </summary>
        public event EventHandler<CacheRemoveEventArgs> OnRemove;

        #endregion

        #region Members

        /// <summary>
        /// a cache map show how cache items saved
        /// </summary>
        internal protected XNodeStore CacheMap { get; private set; }

        /// <summary>
        /// a cache provider get and set cache items
        /// </summary>
        public abstract ICacheProvider CacheProvider { get; }

        #endregion

        #region Constructors

        public AbstractedCache(string cacheName)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(String.Format("<{0} />", cacheName));

            this.CacheMap = new XNodeStore(xmlDoc.DocumentElement);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取指定路径的缓存
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public virtual object Retrieve(string xpath)
        {
            CacheNode cacheNode = new CacheNode(xpath, this);

            return cacheNode.CachedObject;
        }

        /// <summary>
        /// 级联获取所有被缓存的对象
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public virtual IList<object> RetrieveCascaded(string xpath)
        {
            XNodeList nodes = CacheMap.GetNodes(xpath);

            IList<object> objs = new List<object>();

            if (nodes != null)
            {
                nodes.All((n) =>
                {
                    if (!CacheNode.IsNullOrEmpty(n))
                    {
                        object tobj = Retrieve(n);
                        objs.Add(tobj);
                    }

                    return true;
                });
            }

            return objs;
        }

        /// <summary>
        /// 设置缓存对象
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="obj"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual string Set(string xpath, object obj, object config = null)
        {
            CacheNode cacheNode = new CacheNode(xpath, this, obj);

            if (this.OnSet != null)
            {
                this.OnSet(this, new CacheEventArgs(cacheNode, config));
            }

            cacheNode.Set(config);

            return cacheNode.Id;
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="xpath"></param>
        public virtual void Remove(string xpath)
        {
            CacheNode cacheNode = new CacheNode(xpath, this);

            if (this.OnRemove != null)
            {
                this.OnRemove(this, new CacheRemoveEventArgs(cacheNode));
            }

            cacheNode.Remove();
        }

        /// <summary>
        /// 级联移除缓存对象
        /// </summary>
        /// <param name="xpath"></param>
        public virtual void RemoveCascaded(string xpath)
        {
            CacheNode cacheNode = new CacheNode(xpath, this);

            this.OnRemove(this, new CacheRemoveEventArgs(cacheNode, true));

            cacheNode.RemoveCascaded();
        }

        #endregion

        #region Private Methods

        private object Retrieve(XNode node)
        {
            string cacheId = CacheNode.GetCacheId(node);

            object obj = CacheProvider.Retrieve(cacheId);

            return obj;
        }

        #endregion

    }
}
