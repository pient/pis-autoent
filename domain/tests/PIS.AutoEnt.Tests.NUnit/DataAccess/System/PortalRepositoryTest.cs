using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate;
using NHibernate.Criterion;
using PIS.Framework.Model;
using PIS.Framework.DataAccess;
using PIS.Framework.DataAccess.Interfaces;

namespace PIS.Framework.Tests.DataAccess.System
{
    [TestFixture]
    public class PortalRepositoryTest
    {
        [SetUp]
        public void Setup()
        {
            AppSystem.Initialize();
        }

        [Test]
        public void GetMenu()
        {
            IPortalRepository repository = ObjectManager.Resolve<IPortalRepository>();

            SysPortalMenu menu = repository.GetMenu();

            Assert.IsNotNull(menu);
        }
    }
}
