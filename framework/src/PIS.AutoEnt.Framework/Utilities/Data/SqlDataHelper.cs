using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;

namespace PIS.AutoEnt.Data
{
    public static class SqlDataHelper
    {
        #region ExecuteNonQuery

        public static int ExecuteNonQuery(string connectionString, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(connectionString, CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static int ExecuteNonQuery(SqlConnection connection, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(connection, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlConnection connection, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static int ExecuteNonQuery(SqlTransaction trans, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(trans, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        #endregion

        #region ExecuteReader

        public static SqlDataReader ExecuteReader(string connectionString, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(connectionString, CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        #endregion

        #region ExecuteScalar

        public static object ExecuteScalar(string connectionString, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(connectionString, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static object ExecuteScalar(SqlConnection connection, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(connection, cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(SqlConnection connection, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        #endregion

        #region Execute DataTable or DataSet

        /// <summary>
        /// 返回TableName的主键，单主键
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetPrimaryKey(string connectionString, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return GetPrimaryKey(connection, tableName);
            }
        }

        /// <summary>
        /// 返回TableName的主键，单主键
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetPrimaryKey(SqlConnection connection, string tableName)
        {
            DataTable schema = ExecuteDataSchema(connection, tableName);

            return GetPrimaryKey(schema);
        }

        /// <summary>
        /// 返回DataTable的主键，单主键
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static string GetPrimaryKey(DataTable dataTable)
        {
            return dataTable.PrimaryKey.First().ColumnName;
        }

        /// <summary>
        /// 获取表或视图的Schema
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataSchema(string connectionString, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteDataTable(connection, tableName);
            }
        }

        /// <summary>
        /// 获取表或视图的Schema
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataSchema(SqlConnection connection, string tableName)
        {
            string schemasql = String.Format("SELECT * FROM {0} WHERE 1=0", tableName);

            DataTable dt = ExecuteDataTable(connection, schemasql);

            return dt;
        }

        /// <summary>
        /// 执行语句并返回DataTable
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string connectionString, string cmdText)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteDataTable(connection, cmdText);
            }
        }

        /// <summary>
        /// 执行语句并返回DataTable
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(SqlConnection connection, string cmdText)
        {
            SqlDataAdapter sqlDA = new SqlDataAdapter(cmdText, connection);
            DataTable dt = new DataTable();

            sqlDA.Fill(dt);

            return dt;
        }

        /// <summary>
        /// 执行语句并返回DataSet
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string connectionString, string cmdText)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteDataSet(connection, cmdText);
            }
        }

        /// <summary>
        /// 执行语句并返回DataSet
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(SqlConnection connection, string cmdText)
        {
            var sqlDA = new SqlDataAdapter(cmdText, connection);
            DataSet ds = new DataSet();

            sqlDA.Fill(ds);

            return ds;
        }

        #endregion

        #region Execute Object

        /// <summary>
        /// 执行语句并返回指定类型对象列表
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static IList<T> ExecuteObjects<T>(string connectionString, string cmdText) where T : new()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteObjects<T>(connection, cmdText);
            }
        }

        /// <summary>
        /// 执行语句并返回指定类型对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static IList<T> ExecuteObjects<T>(SqlConnection connection, string cmdText) where T : new()
        {
            SqlDataAdapter sqlDA = new SqlDataAdapter(cmdText, connection);
            DataTable dt = new DataTable();

            sqlDA.Fill(dt);

            return dt.ToList<T>();
        }

        /// <summary>
        /// 执行语句并返回指定类型对象列表
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static T ExecuteObject<T>(string connectionString, string cmdText) where T : new()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteObject<T>(connection, cmdText);
            }
        }

        /// <summary>
        /// 执行获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static T ExecuteObject<T>(SqlConnection connection, string cmdText) where T : new()
        {
            string _queryText = String.Format("SELECT TOP 1 * FROM ({0}) __a", cmdText);
            SqlDataAdapter sqlDA = new SqlDataAdapter(cmdText, connection);
            DataTable dt = new DataTable();

            sqlDA.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return default(T);
            }
            else
            {
                return dt.ToList<T>().FirstOrDefault();
            }
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (var parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion
    }
}
