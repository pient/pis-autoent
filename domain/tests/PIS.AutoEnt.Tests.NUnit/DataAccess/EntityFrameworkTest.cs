using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using NUnit.Framework;
using PIS.Framework.Model;
using PIS.Framework.DataAccess;

namespace PIS.Framework.Tests.DataAccess
{
    [TestFixture]
    public class EntityFrameworkTest
    {
        [SetUp]
        public void Setup()
        {
            AppSystem.Initialize();
        }

        [Test]
        public void SimpleTest()
        {
            using (var dbContext = new SysDbContext())
            {
                // SysRegistry reg = dbContext.SysRegistry.FirstOrDefault();

                // Assert.IsNotNull(reg);
            }
        }
    }
}
