using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Tests.DataAccess
{
    [TestClass]
    public class ModelHelperTest
    {
        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
        }

        [TestMethod]
        public void Domain_ModelHelper_BuildEntitySqlConnectionTest()
        {
            DataContextHelper.BuildEntitySqlConnection(AppDataAccessor.ConnectionString);
        }
    }
}
