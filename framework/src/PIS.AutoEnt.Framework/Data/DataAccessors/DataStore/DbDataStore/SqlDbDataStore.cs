using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PIS.AutoEnt.Data
{
    public class SqlDbDataStore : DbDataStore
    {
        #region 变量属性 

        private SqlConnection _Connection = new SqlConnection();
        private SqlConnection _TransConnection = new SqlConnection();
        private SqlTransaction _Transaction = null;
        private string _ConnectionString = null;

        /// <summary>
        /// 连接字符串 
        /// </summary>
        public override string ConnectionString
        {
            get { return _ConnectionString; }
        }

        public override IDbConnection Connection
        {
            get { return _Connection; }
        }

        public override IDbConnection TransConnection
        {
            get { return _TransConnection; }
        }

        public override IDbTransaction Transaction
        {
            get { return _Transaction; }
        }

        #endregion

        #region 构造函数

        public SqlDbDataStore(string connString)
        {
            this._Connection.ConnectionString = connString;
            this._TransConnection.ConnectionString = connString;

            this._ConnectionString = connString;
        }

        public SqlDbDataStore(SqlConnection sqlConn)
        {
            this._Connection = sqlConn;

            this._ConnectionString = sqlConn.ConnectionString;
            this._TransConnection.ConnectionString = sqlConn.ConnectionString;
        }

        public SqlDbDataStore(SqlTransaction sqlTrans)
        {
            this._Connection.ConnectionString = sqlTrans.Connection.ConnectionString;

            this._Transaction = sqlTrans;
            this._TransConnection = sqlTrans.Connection;
        }

        #endregion

        #region DataStore 成员

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="isolationLevel"></param>
        public override void BeginTransaction(IsolationLevel isolationLevel)
        {
            this.IsolationLevel = isolationLevel;

            if (_Transaction == null)
            {
                this.TransConnection.Open();
                _Transaction = (SqlTransaction)this.TransConnection.BeginTransaction(isolationLevel);
            }
            else
            {
                throw new PISDataException();
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public override void CommitTransaction()
        {
            base.CommitTransaction();

            _Transaction = null;
        }

        /// <summary>
        /// 回滚事务处理
        /// </summary>
        public override void RollbackTrans()
        {
            base.RollbackTrans();

            _Transaction = null;
        }

        public override int ExecuteNonQuery(string cmdText, CommandType cmdType = CommandType.Text, params IDbDataParameter[] cmdParameters)
        {
            SqlParameter[] parameters = cmdParameters.Select(p => { return (SqlParameter)p; }).ToArray();

            return SqlDataHelper.ExecuteNonQuery(this._Connection, cmdText, cmdType, parameters);
        }

        /// <summary>
        /// 批量执行命令并提交
        /// </summary>
        /// <param name="cmdTexts"></param>
        /// <returns></returns>
        public override int ExecuteTransNonQueryAndCommit(params string[] cmdTexts)
        {
            try
            {
                int affected = 0;
                foreach (string _cmdText in cmdTexts)
                {
                    affected += SqlDataHelper.ExecuteNonQuery(this._TransConnection, _cmdText);
                }

                CommitTransaction();

                return affected;
            }
            catch (PISDataException ex)
            {
                this.RollbackTrans();

                throw ex;
            }
            catch (Exception ex)
            {
                this.RollbackTrans();

                throw new PISDataException(ex);
            }
        }

        public override int ExecuteTransNonQuery(string cmdText, CommandType cmdType = CommandType.Text, params IDbDataParameter[] cmdParameters)
        {
            SqlParameter[] parameters = cmdParameters.Select(p => { return (SqlParameter)p; }).ToArray();

            return SqlDataHelper.ExecuteNonQuery(this._TransConnection, cmdText, cmdType, parameters);
        }

        public override object ExecuteScalar(string cmdText, CommandType cmdType = CommandType.Text, params IDbDataParameter[] cmdParameters)
        {
            SqlParameter[] parameters = cmdParameters.Select(p => { return (SqlParameter)p; }).ToArray();

            return SqlDataHelper.ExecuteScalar(this._Connection, cmdText, cmdType, parameters);
        }

        public override DataTable ExecuteDataSchema(string tableName)
        {
            return SqlDataHelper.ExecuteDataSchema(this._Connection, tableName);
        }

        public override DataTable ExecuteDataTable(string cmdText)
        {
            return SqlDataHelper.ExecuteDataTable(this._Connection, cmdText);
        }

        public override DataSet ExecuteDataSet(string cmdText)
        {
            return SqlDataHelper.ExecuteDataSet(this._Connection, cmdText);
        }

        public override IList<T> ExecuteObjects<T>(string cmdText)
        {
            return SqlDataHelper.ExecuteObjects<T>(this._Connection, cmdText);
        }

        public override T ExecuteObject<T>(string cmdText)
        {
            return SqlDataHelper.ExecuteObject<T>(this._Connection, cmdText);
        }

        public override void Dispose()
        {
            base.Dispose();

            this._Connection = null;
            this._Transaction = null;
            this._TransConnection = null;
        }

        #endregion
    }
}
