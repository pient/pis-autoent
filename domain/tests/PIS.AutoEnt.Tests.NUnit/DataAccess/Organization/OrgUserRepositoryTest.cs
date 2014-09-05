using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate;
using NHibernate.Criterion;
using PIS.Framework.Model;
using PIS.Framework.DataAccess;

namespace PIS.Framework.Tests.DataAccess.Organization
{
    [TestFixture]
    public class OrgUserRepositoryTest
    {
        const string TEST_USER_ID = "46c5f4df-f6d1-4b36-96ac-d39d3dd65a5d";
        const string TEST_USER_NAME = "admin";

        [SetUp]
        public void Setup()
        {
            AppSystem.Initialize();
        }

        [Test]
        public void Get()
        {
            OrgUserRepository repository = new OrgUserRepository();

            // OrgUser orgUser = repository.Get(TEST_USER_ID);

            // Assert.AreEqual(orgUser.Name, TEST_USER_NAME);
        }
    }
}
