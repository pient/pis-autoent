using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.XData
{
    /// <summary>
    /// XData标签，用于获取具体XData数据
    /// </summary>
    public class XDataMeta : FieldMeta
    {
        #region 成员属性

        public object Id
        {
            get;
            set;
        }

        public string RootPath
        {
            get;
            set;
        }

        public string RootName
        {
            get;
            set;
        }

        public string RootNodeString
        {
            get
            {
                return String.Format("<{0} />", this.RootName);
            }
        }

        #endregion

        #region 构造函数

        public XDataMeta()
            : base(SysConsts.DefaultXDataField, SysConsts.DefaultSysObjectTable, SysConsts.DefaultIdField, AppDataAccessor.ConnectionString)
        {
            this.RootName = SysConsts.DefaultXDataRootName;
            this.RootPath = SysConsts.DefaultXDataRootName;
        }

        public XDataMeta(object id)
            : this()
        {
            this.Id = id;
        }

        public XDataMeta(object id, string table)
            : this(id)
        {
            this.Table = table;
        }

        public XDataMeta(object id, string table, string idField)
            : this(id)
        {
            this.Table = table;
            this.IdField = idField;
        }

        public XDataMeta(object id, string table, string idField, string rootPath)
            : this(id, table, idField)
        {
            this.RootPath = rootPath;
        }

        #endregion
    }
}
