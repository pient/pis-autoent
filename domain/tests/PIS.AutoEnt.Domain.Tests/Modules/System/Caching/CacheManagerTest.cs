using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace PIS.AutoEnt.Framework.Tests.Modules.Modules.System.Caching
{
    [TestClass]
    public class CacheManagerTest
    {
        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
        }

        [TestMethod]
        public void Domain_CacheManager_SystemCache_PolicyTest()
        {
            CacheItemPolicy policy = new CacheItemPolicy()
            {
                 SlidingExpiration = TimeSpan.FromSeconds(3)
            };

            string objAPath = @"./Temp_ObjA";
            object objAValue = new { A = "A1", B = "B2" };

            CacheManager.SystemCache.Set(objAPath, objAValue, policy);
            
            Thread.Sleep(1000);
            object objACached = CacheManager.SystemCache.Retrieve(objAPath);

            Assert.IsNotNull(objACached);

            Thread.Sleep(3000);
            objACached = CacheManager.SystemCache.Retrieve(objAPath);
            Assert.IsNull(objACached);
        }
    }
}
