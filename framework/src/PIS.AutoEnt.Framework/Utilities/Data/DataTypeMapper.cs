using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PIS.AutoEnt.Data
{
    /// <summary>
    /// 数据库及不同语言见数据类型的映射关系
    /// </summary>
    public static class DataTypeMapper
    {
        static HashSet<DbTypeMapItem> DbTypeMap;

        static DataTypeMapper()
        {
            DbTypeMap = new HashSet<DbTypeMapItem>();

            Initialize();
        }

        static void Initialize()
        {
            // 设置SQL类型到C#类型映射
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Bit, "bit", typeof(Boolean), "bool", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.TinyInt, "tinyint", typeof(Byte), "byte", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.SmallInt, "smallint", typeof(Int16), "Int16", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Int, "int", typeof(Int32), "int", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.BigInt, "bigint", typeof(Int64), "Int64", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.SmallMoney, "smallmoney", typeof(Decimal), "decimal"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Money, "money", typeof(Decimal), "decimal"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Decimal, "decimal", typeof(Decimal), "decimal", true));
            // DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Numeric, "numeric", typeof(Decimal), "decimal"));

            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Float, "float", typeof(Double), "double", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Real, "real", typeof(Single), "Single", true));

            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.SmallDateTime, "smalldatetime", typeof(DateTime), "DateTime"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Date, "date", typeof(DateTime), "DateTime"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.DateTime, "datetime", typeof(DateTime), "DateTime", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.DateTime2, "datetime2", typeof(DateTime), "DateTime"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Timestamp, "timestamp", typeof(DateTime), "DateTime"));

            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Char, "char", typeof(String), "String"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Text, "text", typeof(String), "String"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.VarChar, "varchar", typeof(String), "String"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.NChar, "nchar", typeof(String), "String"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.NText, "ntext", typeof(String), "String"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.NVarChar, "nvarchar", typeof(String), "String", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Xml, "xml", typeof(String), "String"));

            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Binary, "binary", typeof(Byte[]), "Byte[]"));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.VarBinary, "varbinary", typeof(Byte[]), "Byte[]", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Image, "image", typeof(Byte[]), "Byte[]"));

            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.UniqueIdentifier, "uniqueidentifier", typeof(Guid), "Guid", true));
            DbTypeMap.Add(new DbTypeMapItem(SqlDbType.Variant, "variant", typeof(Object), "Object", true));
        }

        // 获取映射项
        public static DbTypeMapItem GetMapItem(SqlDbType sqlDbType)
        {
            DbTypeMapItem mapItem = DbTypeMap.Where(v => v.SqlDbType == sqlDbType).FirstOrDefault();
            return mapItem;
        }

        public static SqlDbType GetSqlType(Type type)
        {
            DbTypeMapItem mapItem = DbTypeMap.Where(v => (v.CSharpType == type && v.DefaultSqlDbType == true)).FirstOrDefault();

            if (mapItem == null)
            {
                return SqlDbType.Variant;
            }
            else
            {
                return mapItem.SqlDbType;
            }
        }

        public static string GetSqlTypeString(Type type)
        {
            DbTypeMapItem mapItem = DbTypeMap.Where(v => (v.CSharpType == type && v.DefaultSqlDbType == true)).FirstOrDefault();
            
            if (mapItem == null)
            {
                return "nvarchar(max)";
            }
            else
            {
                return mapItem.SqlDbTypeString;
            }
        }
    }

    public class DbTypeMapItem
    {
        #region 属性

        public SqlDbType SqlDbType
        {
            get;
            private set;
        }

        public string SqlDbTypeString
        {
            get;
            private set;
        }

        public Type CSharpType
        {
            get;
            private set;
        }

        public string CSharpTypeString
        {
            get;
            private set;
        }

        public bool DefaultSqlDbType
        {
            get;
            private set;
        }

        #endregion

        #region 构造函数

        internal DbTypeMapItem()
        {
        }

        internal DbTypeMapItem(SqlDbType sqlDbType, string sqlDbTypeString, Type cSharpType, string cSharpTypeString, bool defaultSqlDbType = false)
        {
            this.SqlDbType = sqlDbType;
            this.SqlDbTypeString = sqlDbTypeString;
            this.CSharpType = cSharpType;
            this.CSharpTypeString = cSharpTypeString;
            this.DefaultSqlDbType = defaultSqlDbType;
        }

        #endregion
    }
}
