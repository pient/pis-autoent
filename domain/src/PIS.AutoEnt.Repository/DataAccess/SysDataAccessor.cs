using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Data.Query;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public class SysDataAccessor
    {
        #region Unit Of Work

        public static IQueryBuilder QueryBuilder
        {
            get
            {
                return new SqlQueryBuilder();

            }
        }

        public static ISysUnitOfWork NewUnitOfWork()
        {
            return ObjectFactory.Resolve<ISysUnitOfWork>();
        }

        #endregion

        #region SQL

        public static QueryResult QueryData(QueryExpr qryExpr)
        {
            var qryResult = new QueryResult();

            var sql = QueryBuilder.GetQueryString(qryExpr);

            using (var ctx = new SysDbContext())
            {
                var _conn = ctx.Database.Connection as SqlConnection;

                qryResult.Data = SqlDataHelper.ExecuteDataTable(_conn, sql);

                if (qryExpr.GetTotalCount)
                {
                    var count_sql = QueryBuilder.GetRecordCountSQLString(qryExpr);

                    var recCount = SqlDataHelper.ExecuteScalar(_conn, count_sql);

                    qryResult.TotalCount = (int)recCount;
                }

                return qryResult;
            }
        }

        /// <summary>
        /// 获取结构数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="structureCode"></param>
        /// <param name="sCondition"></param>
        /// <param name="tCondition"></param>
        /// <returns></returns>
        public static DataTable GetStructuredData(string tableName, string sCondition = null, string tCondition = null, 
            int? rootLevel = null, string structureCode = null)
        {
            if (structureCode == null)
            {
                structureCode = tableName;
            }

            if (!String.IsNullOrEmpty(sCondition))
            {
                sCondition = ("N'" + sCondition.Replace("'", "''") + "'");
            }
            else
            {
                sCondition = "NULL";
            }

            if (!String.IsNullOrEmpty(tCondition))
            {
                tCondition = ("N'" + tCondition.Replace("'", "''") + "'");
            }
            else
            {
                tCondition = "NULL";
            }

            string sql = String.Format("exec [uspGetStructuredData] N'{0}', N'{1}',{2}, {3}", 
                tableName, structureCode, sCondition, tCondition);

            DataTable dt = new DataTable();

            using (var ctx = new SysDbContext())
            {
                var _conn = ctx.Database.Connection as SqlConnection;

                dt = SqlDataHelper.ExecuteDataTable(_conn, sql);
            }

            if (rootLevel != null)
            {
                var rootRows = dt.Select("PathLevel = " + rootLevel);

                // 清空ParentId
                foreach (var row in rootRows)
                {
                    row["ParentId"] = DBNull.Value;
                }
            }

            return dt;
        }

        #endregion
    }
}
