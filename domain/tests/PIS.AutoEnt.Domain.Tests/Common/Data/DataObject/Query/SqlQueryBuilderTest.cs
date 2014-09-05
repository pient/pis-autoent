using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Data.Query;

namespace PIS.AutoEnt.Tests.Common.Data.DataObject.Query
{
    [TestClass]
    public class SqlQueryBuilderTest
    {
        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
        }

        [TestMethod]
        public void Domain_SqlQueryBuilder_GetQueryStringTest()
        {
            QueryExpr qryexpr = new QueryExpr();
            // qryexpr.QueryObject = new QueryObject("SysObject", QueryObjectType.Table);
            qryexpr.Target = new QueryTarget("SELECT * FROM SysObject", QueryTargetType.QueryString);

            qryexpr.QueryFields.Add(new QueryField(SysConsts.DefaultIdField));
            qryexpr.QueryFields.Add(new QueryField("Code"));
            qryexpr.QueryFields.Add(new QueryField("Name"));

            qryexpr.Conditions.Add("Code", "T", ConditionType.Like);
            qryexpr.Conditions.Add("Name", ConditionType.IsNotNull);

            qryexpr.Sorters.Add(new QryOrder(SysConsts.DefaultIdField));

            IQueryBuilder qrybuilder = new SqlQueryBuilder();

            string qrystr = qrybuilder.GetQueryString(qryexpr);

            SqlDataReader reader = SqlDataHelper.ExecuteReader(AppDataAccessor.ConnectionString, qrystr);

            while (reader.Read())
            {
                Console.Write(String.Format("Code: {0}, Name: {1}", reader["Code"], reader["Name"]));
            }

            reader.Close();

            IList<MyObject> t_objs = SqlDataHelper.ExecuteObjects<MyObject>(AppDataAccessor.ConnectionString, qrystr);

            MyObject t_obj = SqlDataHelper.ExecuteObject<MyObject>(AppDataAccessor.ConnectionString, qrystr);
        }

        #region Support Classes

        public class MyObject
        {
            public Guid ObjectId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }

        #endregion
    }
}
