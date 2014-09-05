using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PIS.AutoEnt.Data
{
    /// <summary>
    /// 数据库DataStore, 用于普通查询
    /// </summary>
    public abstract class DbDataStore : IDataStorage
    {
        #region 成员属性

        /// <summary>
        /// 数据库连接
        /// </summary>
        public abstract IDbConnection Connection { get; }

        /// <summary>
        /// 数据库事务连接
        /// </summary>
        public abstract IDbConnection TransConnection { get; }

        /// <summary>
        /// 数据库事务
        /// </summary>
        public abstract IDbTransaction Transaction { get; }

        /// <summary>
        /// 连接字符串 
        /// </summary>
        public abstract string ConnectionString { get; }

        /// <summary>
        /// 隔离级别
        /// </summary>
        public virtual IsolationLevel IsolationLevel { get; protected set; }

        #endregion

        #region 构造函数

        protected DbDataStore()
        {
            this.IsolationLevel = System.Data.IsolationLevel.Unspecified;
        }

        #endregion

        #region 公共成员

        /// <summary>
        /// 打开连接
        /// </summary>
        public virtual void OpenConnection()
        {
            this.Connection.Open();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public virtual void CloseConnection()
        {
            this.Connection.Close();
        }

        /// <summary>
        /// 打开事务连接
        /// </summary>
        public virtual void OpenTransConnection()
        {
            if (this.TransConnection == null)
            {
                throw new PISDataException("transaction connection does not exist.");
            }

            if (this.TransConnection.State == ConnectionState.Closed)
            {
                this.TransConnection.Open();
            }
        }

        /// <summary>
        /// 关闭事务连接
        /// </summary>
        public virtual void CloseTransConnection()
        {
            if (this.TransConnection == null)
            {
                throw new PISDataException("transaction connection does not exist.");
            }

            if (this.TransConnection.State != ConnectionState.Closed)
            {
                this.TransConnection.Close();
            }
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public virtual void BeginTransaction()
        {
            this.BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="isolationLevel"></param>
        public abstract void BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void CommitTransaction()
        {
            if (Transaction == null)
            {
                throw new PISDataException();
            }
            else
            {
                try
                {
                    Transaction.Commit();
                    Transaction.Dispose();
                }
                catch (Exception ex)
                {
                    new PISDataException(ex);
                }
                finally
                {
                    this.TransConnection.Close();
                }
            }
        }

        /// <summary>
        /// 回滚事务处理
        /// </summary>
        public virtual void RollbackTrans()
        {
            if (Transaction == null)
            {
                throw new PISDataException();
            }
            else
            {
                try
                {
                    Transaction.Rollback();
                    Transaction.Dispose();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    this.TransConnection.Close();
                }
            }
        }

        public abstract int ExecuteNonQuery(string cmdText, CommandType cmdType = CommandType.Text, params IDbDataParameter[] cmdParameters);

        public abstract int ExecuteTransNonQuery(string cmdText, CommandType cmdType = CommandType.Text, params IDbDataParameter[] cmdParameters);

        public abstract int ExecuteTransNonQueryAndCommit(params string[] cmds);

        public abstract object ExecuteScalar(string cmdText, CommandType cmdType = CommandType.Text, params IDbDataParameter[] cmdParameters);

        public abstract DataTable ExecuteDataSchema(string tableName);

        public abstract DataTable ExecuteDataTable(string cmdText);

        public abstract DataSet ExecuteDataSet(string cmdText);

        public abstract IList<T> ExecuteObjects<T>(string cmdText) where T : new();

        public abstract T ExecuteObject<T>(string cmdText) where T : new();

        #endregion

        #region IDataStore成员

        public virtual void Dispose()
        {
            if (this.Connection != null)
            {
                this.Connection.Dispose();
            }

            if (this.TransConnection != null)
            {
                this.Connection.Dispose();
            }

            if (this.Transaction != null)
            {
                this.Transaction.Dispose();
            }
        }

        #endregion
    }
}
