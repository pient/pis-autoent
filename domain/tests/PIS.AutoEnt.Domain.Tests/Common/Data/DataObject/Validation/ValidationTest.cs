using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Framework.Tests.Common.Data.DataObject.Validation
{
    [TestClass]
    public class ValidationTest
    {
        private const string ValidCode = "TestUser";
        private const string LongCode = "TestName__TestName__TestName__TestName__TestName__TestName__TestName__TestName";

        private const string ValidName = "TestName";

        private const string ValidEmail = "abc@pis.com";
        private const string InvalidEmail = "pis.com";

        [TestInitialize]
        public void Setup()
        {
            Sys.AppInitializer.Initialize();
        }

        // 目前Mvc Validation 不管用
        // [TestMethod]
        public void Domain_UserModelValidation_Test()
        {
            OrgUser orgUser = new OrgUser();

            ValidationResults results = orgUser.IsValid();
            Assert.IsFalse(results.IsValid());

            orgUser.Code = ValidCode;
            orgUser.Name = ValidName;
            results = orgUser.IsValid();
            Assert.IsTrue(results.IsValid());

            orgUser.Code = LongCode;
            results = orgUser.IsValid();
            Assert.IsFalse(results.IsValid());

            orgUser.Code = ValidCode;
            orgUser.Email = InvalidEmail;
            results = orgUser.IsValid();
            Assert.IsFalse(results.IsValid());

            orgUser.Email = ValidEmail;
            results = orgUser.IsValid();
            Assert.IsTrue(results.IsValid());
        }
    }
}
