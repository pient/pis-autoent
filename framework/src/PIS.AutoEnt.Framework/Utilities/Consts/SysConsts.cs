using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 系统静态变量
    /// </summary>
    public static class SysConsts
    {
        #region 系统相关

        /// <summary>
        /// Windsor配置段名
        /// </summary>
        public const string WindsorSectionName = "Windsor";

        /// <summary>
        /// 默认日志名
        /// </summary>
        public const string DefaultLoggerName = "Default";

        /// <summary>
        /// 语言信息，在Session等处保存的键名
        /// </summary>
        public const string LanguageKey = "Language";

        /// <summary>
        /// 用户名信息，在Session等处保存的键名
        /// </summary>
        public const string UserKey = "User";

        #endregion

        #region 数据操作相关

        public const string DefaultNumberRowName = "__sort_row_number";

        /// <summary>
        /// 默认主键字段
        /// </summary>
        public const string DefaultIdField = "Id";

        /// <summary>
        /// 默认系统对象表
        /// </summary>
        public const string DefaultSysObjectTable = "SysObject";

        /// <summary>
        /// 默认XDataObject根路径
        /// </summary>
        public const string DefaultXDataRootName = "XData";

        /// <summary>
        /// 默认XDataObject数据字段
        /// </summary>
        public const string DefaultXDataField = "XData";


        #endregion
    }
}
