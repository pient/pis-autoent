using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Config;
using PIS.AutoEnt.Data.Query;
using PIS.AutoEnt.XData;
using System.Collections;

namespace PIS.AutoEnt.Data
{
    public class DataConnections
    {
        #region Consts

        public const string Framework = "Framework";    // 系统连接名
        public const string Business = "Business";      // 业务库连接名
        public const string Temporary = "Temporary";    // 临时库连接名

        #endregion
    }

    public sealed class AppDataAccessor
    {
        #region ConnectionStrings

        // 系统连接字符串
        public static string ConnectionString
        {
            get
            {
                string conn_str = String.Empty;
                ConfigManager.ConnectionStrings.TryGetValue(DataConnections.Framework, out conn_str);

                return conn_str;
            }
        }

        // 临时库连接字符串
        public static string BizConnectionString
        {
            get
            {
                string conn_str = String.Empty;
                ConfigManager.ConnectionStrings.TryGetValue(DataConnections.Business, out conn_str);

                return conn_str;
            }
        }

        // 临时库连接字符串
        public static string TempConnectionString
        {
            get
            {
                string conn_str = String.Empty;
                ConfigManager.ConnectionStrings.TryGetValue(DataConnections.Temporary, out conn_str);

                return conn_str;
            }
        }

        #endregion

        #region DataStores

        /// <summary>
        /// 创建DataStore 
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static DbDataStore NewDataStore(string connString)
        {
            return new SqlDbDataStore(connString);
        }

        /// <summary>
        /// 创建DataStore 
        /// </summary>
        /// <returns></returns>
        public static DbDataStore NewDataStore()
        {
            return NewDataStore(AppDataAccessor.ConnectionString);
        }

        /// <summary>
        /// 创建XDataStore
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="dataStore"></param>
        /// <returns></returns>
        public static IXDataStorage NewXDataStore(XDataMeta metadata, DbDataStore dataStore = null)
        {
            if (dataStore != null)
            {
                return new XSqlDataStorage(metadata, dataStore);
            }
            else
            {
                if (String.IsNullOrWhiteSpace(metadata.ConnectionString))
                {
                    metadata.ConnectionString = AppDataAccessor.ConnectionString;
                }

                return new XSqlDataStorage(metadata);
            }
        }

        #endregion

        #region Utility Methods

        public static ISysUnitOfWork NewUnitOfWork()
        {
            return ObjectFactory.Resolve<ISysUnitOfWork>();
        }

        public static T NewUnitOfWork<T>() where T : IUnitOfWork
        {
            return ObjectFactory.Resolve<T>();
        }

        public static T GetRepository<T>(IEntityContext ctx = null)
        {
            if (ctx == null)
            {
                return ObjectFactory.Resolve<T>();
            }
            else
            {
                EasyDictionary<IEntityContext> ctxDict = new EasyDictionary<IEntityContext>();
                ctxDict.Set("ctx", ctx);

                return ObjectFactory.Resolve<T>(ctxDict);
            }
        }

        public static IQueryBuilder GetQueryBuilder()
        {
            return new SqlQueryBuilder();
        }

        #endregion
    }
}
