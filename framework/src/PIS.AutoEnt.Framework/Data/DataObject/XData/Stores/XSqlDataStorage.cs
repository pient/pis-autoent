using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.XData
{
    public class XSqlDataStorage : IXDataStorage
    {
        #region 成员属性

        public const string QRY_QRY_FORMAT = "SELECT {0}.query('{1}') FROM {2} WHERE {3} = @Id";
        public const string EXT_QRY_FORMAT = "SELECT {0}.exist('{1}') FROM {2} WHERE {3} = @Id";
        public const string VAL_QRY_FORMAT = "SELECT {0}.value('{1}', '{2}') FROM {3}  WHERE {4}=@Id";
        public const string MDY_QRY_FORMAT = "UPDATE {0} SET {1}.modify('{2}') WHERE {3} = @Id";

        /// <summary>
        /// XData元数据
        /// </summary>
        public XDataMeta Metadata
        {
            get;
            protected set;
        }

        public DbDataStore DataStore { get; protected set; }

        #endregion

        #region 构造函数

        public XSqlDataStorage(XDataMeta metadata)
        {
            this.Metadata = metadata;

            this.DataStore = new SqlDbDataStore(metadata.ConnectionString);
        }

        public XSqlDataStorage(XDataMeta metadata, SqlConnection sqlConn)
        {
            if (String.IsNullOrWhiteSpace(metadata.ConnectionString))
            {
                metadata.ConnectionString = sqlConn.ConnectionString;
            }
            else if (String.Compare(this.Metadata.ConnectionString, sqlConn.ConnectionString, true) != 0)
            {
                throw new PISDataException("metadata connection string and sqlConn connection string doesn't match.");
            }

            this.Metadata = metadata;
            this.DataStore = new SqlDbDataStore(sqlConn);
        }

        public XSqlDataStorage(XDataMeta metadata, DbDataStore dataStore)
        {
            if (String.IsNullOrWhiteSpace(metadata.ConnectionString))
            {
                metadata.ConnectionString = dataStore.ConnectionString;
            }
            else if (String.Compare(metadata.ConnectionString, dataStore.ConnectionString, true) != 0)
            {
                throw new PISDataException("metadata connection string and dataStore connection string doesn't match.");
            }

            this.Metadata = metadata;
            this.DataStore = dataStore;
        }

        #endregion

        #region IXDataObjectProvider成员

        public bool Exists(string xpath)
        {
            if (String.IsNullOrEmpty(xpath))
            {
                return this.ExistsRoot();
            }
            else
            {
                string sqlCmd = String.Format(EXT_QRY_FORMAT, 
                    this.Metadata.DataField, xpath, this.Metadata.Table, this.Metadata.IdField);

                object xdata = DataStore.ExecuteScalar(sqlCmd, CommandType.Text, new SqlParameter("@Id", this.Metadata.Id));

                return (bool)xdata;
            }
        }

        public T Value<T>(string xpath)
        {
            string sqlDbTypeString = GetSqlType(typeof(T));

            string sqlCmd = String.Format(VAL_QRY_FORMAT,
                this.Metadata.DataField, xpath, sqlDbTypeString, this.Metadata.Table, this.Metadata.IdField);

            object xdata = DataStore.ExecuteScalar(sqlCmd, CommandType.Text, new SqlParameter("@Id", this.Metadata.Id));

            if (xdata == null || xdata == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T)xdata;
            }
        }

        public string Query(string xpath)
        {
            string sqlCmd = String.Format(QRY_QRY_FORMAT, this.Metadata.DataField, xpath, this.Metadata.Table, this.Metadata.IdField);

            string xdata = DataStore.ExecuteScalar(sqlCmd, CommandType.Text, new SqlParameter("@Id", this.Metadata.Id)) as string;

            return xdata;
        }

        public void Modify(string xquery)
        {
            string sqlCmd = String.Format(MDY_QRY_FORMAT, this.Metadata.Table, this.Metadata.DataField, xquery, this.Metadata.IdField);

            DataStore.ExecuteNonQuery(sqlCmd, CommandType.Text, new SqlParameter("@Id", this.Metadata.Id));
        }

        #region IXObjectProvider 成员

        public XNode GetRoot()
        {
            return this.GetSingleNode("/");
        }

        public XNode GetSingleNode(string xpath)
        {
            string t_xpath = this.GetSingleSqlXPath(xpath);
            string xdata = null;

            if (IsAttrPath(xpath))
            {
                xdata = this.Value<string>(t_xpath);

                if (xdata == null) return null;
            }
            else
            {
                xdata = this.Query(t_xpath);    // 当t_xpath不存在时，返回""

                if (String.IsNullOrEmpty(xdata)) return null;
            }

            XNode node = new XNode(xdata);
            return node;
        }

        public XNodeList GetNodes(string xpath)
        {
            string t_xpath = this.GetSqlXPath(xpath);
            string xdata = this.Query(t_xpath);

            if (xdata == null)
            {
                return null;
            }
            else
            {
                XNodeList nodes = new XNodeList(xdata);

                return nodes;
            }
        }

        public string GetValue(string xpath)
        {
            string t_xpath, val = null;

            if (IsAttrPath(xpath))
            {
                t_xpath = this.GetSingleSqlXPath(xpath);

                val = this.Value<string>(t_xpath);
            }
            else
            {
                XNode node = GetSingleNode(xpath);

                if (node != null && node.HasChild())
                {
                    val = node.InnerXml;
                }
            }

            return val;
        }

        public void SetValue(string xpath, string value)
        {
            string t_xpath = this.GetSingleSqlXPath(xpath);
            string xquery_r = "", xquery_d = "", xquery_i = "";

            if (this.IsAttrPath(xpath))
            {
                xquery_r = "replace value of " + t_xpath + " with \"" + value + "\"";

                this.Modify(xquery_r);
            }
            else
            {
                XNode node = this.GetSingleNode(xpath);

                if (node != null && node.HasChild())
                {
                    node.InnerXml = value;

                    xquery_i = "insert " + node.OuterXml + " after " + t_xpath;
                    this.Modify(xquery_i);

                    // 删除当前节点
                    xquery_d = "delete " + t_xpath;
                    this.Modify(xquery_d);
                }
                else
                {
                    t_xpath = this.GetParentXPath(t_xpath);
                    xquery_i = "insert " + node.OuterXml + " as last into " + t_xpath;

                    this.Modify(xquery_i);
                }
            }
        }

        /// <summary>
        /// 删除符合xpath条件的所有第一个节点
        /// </summary>
        /// <param name="xpath"></param>
        public void Remove(string xpath)
        {
            string t_xpath = this.GetSingleSqlXPath(xpath);
            string xquery = "delete " + t_xpath;

            this.Modify(xquery);
        }

        /// <summary>
        /// 删除符合xpath条件的所有节点
        /// </summary>
        /// <param name="xpath"></param>
        public void RemoveAll(string xpath)
        {
            string t_xpath = this.GetSqlXPath(xpath);
            string xquery = "delete " + t_xpath;

            this.Modify(xquery);
        }

        public XNode InsertAttr(string refpath, string name, string value, NodePosition position)
        {
            if (position == NodePosition.After || position == NodePosition.Before)
            {
                throw new PISDataException("Doesn't support insert after or before", new NotSupportedException());
            }

            string xquery = "";
            string n_refpath = this.GetSingleSqlXPath(refpath);
            string p_xpath = refpath;

            // Remove the attr if the value is null
            if (value == null)
            {
                if (IsAttrPath(refpath))
                {
                    p_xpath = this.GetParentXPath(refpath);
                }

                p_xpath = this.GetAttrXPath(p_xpath, name);

                Remove(p_xpath);
            }
            else
            {
                xquery = "insert attribute " + name + " {\"" + value + "\"}";

                switch (position)
                {
                    case NodePosition.After:
                        xquery = xquery + " after " + n_refpath;
                        break;
                    case NodePosition.Before:
                        xquery = xquery + " before " + n_refpath;
                        break;
                    case NodePosition.First:
                        xquery = xquery + " as first into " + n_refpath;
                        break;
                    case NodePosition.Last:
                        xquery = xquery + " as last into " + n_refpath;
                        break;
                }
            }

            this.Modify(xquery);

            string xdata = this.Query(p_xpath);

            return new XNode(xdata);
        }

        public XNode InsertEle(string refpath, string name, string value, NodePosition position)
        {
            string xquery = "";
            string n_refpath = this.GetSingleSqlXPath(refpath);

            value = value ?? "";

            string xmlValue = String.Format("<{0}>{1}</{0}>", name, value);

            xquery = "insert " + xmlValue;

            switch (position)
            {
                case NodePosition.After:
                    xquery = xquery + " after " + n_refpath;
                    break;
                case NodePosition.Before:
                    xquery = xquery + " before " + n_refpath;
                    break;
                case NodePosition.First:
                    xquery = xquery + " as first into " + n_refpath;
                    break;
                case NodePosition.Last:
                    xquery = xquery + " as last into " + n_refpath;
                    break;
            }

            this.Modify(xquery);

            return new XNode(xmlValue);
        }

        #endregion

        #endregion

        #region 公有方法

        /// <summary>
        /// 清空XData
        /// </summary>
        public virtual void CleanXData()
        {
            string sqlCmd = String.Format("UPDATE {0} SET {1} = NULL WHERE {3}=@Id",
                this.Metadata.Table, this.Metadata.DataField, this.Metadata.RootNodeString, this.Metadata.IdField);

            DataStore.ExecuteNonQuery(sqlCmd, CommandType.Text, new SqlParameter("@Id", this.Metadata.Id));
        }

        /// <summary>
        /// 为指定记录准备XData
        /// </summary>
        public virtual void PrepareXData()
        {
            string sqlCmd = String.Format("UPDATE {0} SET {1} = '{2}' WHERE {3}=@Id AND {1} IS NULL",
                this.Metadata.Table, this.Metadata.DataField, this.Metadata.RootNodeString, this.Metadata.IdField);

            DataStore.ExecuteNonQuery(sqlCmd, CommandType.Text, new SqlParameter("@Id", this.Metadata.Id));
        }

        /// <summary>
        /// 是否存在根节点
        /// </summary>
        /// <returns></returns>
        public virtual bool ExistsRoot()
        {
            string sqlCmd = String.Format("SELECT {0} FROM {1} WHERE {2}=@Id",
                this.Metadata.DataField, this.Metadata.Table, this.Metadata.IdField); 
            ;
            object xdata = DataStore.ExecuteScalar(sqlCmd, CommandType.Text, new SqlParameter("@Id", this.Metadata.Id));

            return xdata == null;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取选择单个数据的XPath
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private string GetSingleSqlXPath(string xpath)
        {
            string t_xpath = this.GetSqlXPath(xpath);
            
            t_xpath = String.Format("({0})[1]", t_xpath);

            return t_xpath;
        }

        /// <summary>
        /// 获取处理过的XPath
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private string GetSqlXPath(string xpath)
        {
            string t_xpath = (xpath ?? "").TrimEnd('/');

            if (!String.IsNullOrEmpty(t_xpath) && t_xpath.IndexOf('/') != 0)
            {
                t_xpath = "/" + t_xpath;
            }

            if (t_xpath.IndexOf("/" + this.Metadata.RootName) != 0)
            {
                t_xpath = this.Metadata.RootPath + t_xpath;
            }

            return t_xpath;
        }

        /// <summary>
        /// 获取属性路径
        /// </summary>
        /// <param name="refxpath"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetAttrXPath(string refxpath, string name)
        {
            refxpath = refxpath.TrimEnd('/');
            string attr_xpath = refxpath + "/@" + name;

            return attr_xpath;
        }

        /// <summary>
        /// 获取父节点路径
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private string GetParentXPath(string xpath)
        {
            xpath = xpath.TrimEnd('/');

            string p_xpath = xpath;

            if (this.IsAttrPath(xpath))
            {
                p_xpath = xpath.Substring(0, xpath.LastIndexOf("@") - 1);
            }
            else if (xpath.LastIndexOf("/") > 0)
            {
                p_xpath = xpath.Substring(0, xpath.LastIndexOf("/"));
            }

            return p_xpath;
        }

        /// <summary>
        /// 是否属性路径
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private bool IsAttrPath(string xpath)
        {
            if (xpath.IndexOf("@") >= 0)
            {
                return true;
            }

            if (new Regex(@"@\w+$").IsMatch(xpath))
            {
                return true;
            }

            return false;
        }

        private string GetSqlType(Type type)
        {
            string sqlDbTypeString = DataTypeMapper.GetSqlTypeString(type);

            switch (sqlDbTypeString)
            {
                case "varchar":
                case "nvarchar":
                case "varbinary":
                    sqlDbTypeString = sqlDbTypeString + "(max)";
                    break;
                case "decimal":
                    sqlDbTypeString = "decimal(18, 2)";
                    break;
            }

            return sqlDbTypeString;
        }

        #endregion

        public void Dispose()
        {
            if (this.DataStore != null)
            {
                this.DataStore.Dispose();
            }
        }
    }
}
