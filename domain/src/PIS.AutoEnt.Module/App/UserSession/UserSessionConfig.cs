using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Caching;
using PIS.AutoEnt.Config;
using PIS.AutoEnt.Caching;

namespace PIS.AutoEnt
{
    public class UserSessionConfig : ConfigBase
    {
        #region Properties

        public CacheItemPolicy CachePolicy
        {
            get;
            protected set;
        }

        #endregion

        #region Constructors

        private static object lockobj = new object();

        private static UserSessionConfig instance = null;

        internal static UserSessionConfig Instance
        {
            get
            {
                lock (lockobj)
                {
                    if (instance == null)
                    {
                        lock (lockobj)
                        {
                            instance = new UserSessionConfig(CacheManager.SystemCache.Config.SelectSingleNode(@"./" + UserSession.SessionKey));
                        }
                    }
                }

                return instance;
            }
        }

        private UserSessionConfig(XmlNode sections)
            : base(sections)
        {
            Initialize();
        }

        private void Initialize()
        {
            CachePolicy = new CacheItemPolicy();

            string slidingExprStr = this.RetrieveAttributeSetting(@".", "SlidingExpiration");

            if (!String.IsNullOrEmpty(slidingExprStr))
            {
                CachePolicy.SlidingExpiration = TimeSpan.Parse(slidingExprStr);
            }
        }

        #endregion
    }
}
