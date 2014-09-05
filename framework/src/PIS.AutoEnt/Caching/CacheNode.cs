using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt.Caching
{
    /// <summary>
    /// 缓存项的保存节点
    /// </summary>
    public class CacheNode
    {
        #region Consts

        public const string IdName = "id";

        #endregion

        #region Properties

        /// <summary>
        /// Cache项所在Cache容器
        /// </summary>
        internal AbstractedCache OwnerCache
        {
            get;
            private set;
        }

        /// <summary>
        /// 缓存提供者
        /// </summary>
        internal ICacheProvider CacheProvider
        {
            get { return OwnerCache.CacheProvider; }
        }

        /// <summary>
        /// 缓存地图
        /// </summary>
        internal XNodeStore CacheMap
        {
            get { return OwnerCache.CacheMap; }
        }

        public XNode Node
        {
            get;
            protected set;
        }

        private object cachedObject = null;

        public object CachedObject
        {
            get
            {
                if (cachedObject == null)
                {
                    if (!this.IsEmpty)
                    {
                        cachedObject = OwnerCache.CacheProvider.Retrieve(this.Id);
                    }
                }

                return cachedObject;
            }

            protected set
            {
                cachedObject = value;
            }
        }

        /// <summary>
        /// 缓存路径
        /// </summary>
        public string XPath
        {
            get;
            protected set;
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id
        {
            get
            {
                if (this.Node != null)
                {
                    return GetCacheId(this.Node);
                }

                return null;
            }
        }

        /// <summary>
        /// 是否为空节点
        /// </summary>
        public bool IsEmpty
        {
            get { return String.IsNullOrEmpty(this.Id); }
        }

        #endregion

        #region Constructors

        public CacheNode(CacheNode cacheNode)
            : this(cacheNode.XPath, cacheNode.OwnerCache, cacheNode.cachedObject)
        {
        }

        public CacheNode(string xpath, AbstractedCache ownerCache, object cachedObject = null)
        {
            this.cachedObject = cachedObject;
            this.OwnerCache = ownerCache;
            this.XPath = xpath;

            this.ReloadNode();
        }

        #endregion

        #region Methods

        public void Set(object config = null)
        {
            ReloadNode();

            if (!this.IsEmpty)
            {
                CacheProvider.Remove(this.Id);
            }

            string cacheId = NewCacheId();
            XmlNode node = CacheMap.PrepareNode(this.XPath);

            CacheMap.InsertAttr(this.XPath, IdName, cacheId, NodePosition.First);
            CacheProvider.Add(cacheId, this.cachedObject, config);

            ReloadNode();
        }

        /// <summary>
        /// 从缓存容器中移除
        /// </summary>
        public void Remove()
        {
            ReloadNode();

            if (!this.IsEmpty)
            {
                CacheProvider.Remove(this.Id);

                if (this.Node.HasChild())
                {
                    this.Node.RemoveAllAttr();
                }
                else
                {
                    CacheMap.Remove(this.XPath);
                }
            }

            ReloadNode();
        }

        /// <summary>
        /// 从缓存容器中移除，同时移除其下所有节点
        /// </summary>
        public void RemoveCascaded()
        {
            ReloadNode();

            if (this.Node != null)
            {
                RemoveCascadedByNode(this.Node);

                CacheMap.Remove(XPath);
            }

            ReloadNode();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 获取新的缓存标识
        /// </summary>
        /// <returns></returns>
        private string NewCacheId()
        {
            return SystemHelper.NewCombId().ToString();
        }

        /// <summary>
        /// 重新加载节点
        /// </summary>
        private void ReloadNode()
        {
            this.Node = OwnerCache.CacheMap.GetSingleNode(this.XPath);
        }

        /// <summary>
        /// 级联删除节点
        /// </summary>
        /// <param name="node"></param>
        private void RemoveCascadedByNode(XNode node)
        {
            string cacheId = GetCacheId(node);

            if (!String.IsNullOrEmpty(cacheId))
            {
                CacheProvider.Remove(cacheId);
                node.RemoveAllAttr();
            }

            if (node.HasChild())
            {
                XNodeList tNodes = node.GetChildNodes();

                foreach (XNode tNode in tNodes)
                {
                    RemoveCascadedByNode(tNode);
                }
            }
        }

        #endregion

        #region Static Methods

        public static string GetCacheId(XNode node)
        {
            return node.GetAttr(IdName);
        }

        public static bool IsNullOrEmpty(CacheNode cacheNode)
        {
            return cacheNode == null || cacheNode.IsEmpty;
        }

        public static bool IsNullOrEmpty(XNode mapNode)
        {
            return mapNode == null || String.IsNullOrEmpty(mapNode.GetAttr(IdName));
        }

        #endregion
    }
}
