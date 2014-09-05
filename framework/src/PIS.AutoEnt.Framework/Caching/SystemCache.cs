using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using PIS.AutoEnt.Config;

namespace PIS.AutoEnt.Caching
{
    public sealed class SystemCache : AbstractedCache
    {
        #region Consts

        private static ICacheProvider cacheProvider;

        #endregion

        #region Properties

        public XmlNode Config
        {
            get
            {
                return ConfigManager.CacheConfig.GetConfig(CacheNames.System);
            }
        }

        #endregion

        #region Constructors

        public static readonly SystemCache Instance = new SystemCache();

        private SystemCache()
            : base(CacheNames.System)
        {
            XmlAttribute providerTypeAttr = Config.Attributes["CacheProvider"];

            if (providerTypeAttr == null)
            {
                cacheProvider = new MemoryCacheProvider(CacheNames.System, Config);
            }
            else
            {
                var type = Type.GetType(providerTypeAttr.Value);
                cacheProvider = CLRHelper.CreateInstance<ICacheProvider>(type, CacheNames.System, Config);
            }
        }

        #endregion

        #region AbstractedCache Members

        public override ICacheProvider CacheProvider
        {
            get { return cacheProvider; }
        }

        #endregion
    }
}
