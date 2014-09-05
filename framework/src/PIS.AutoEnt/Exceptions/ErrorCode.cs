using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    [Serializable]
    public class ErrorCode
    {
        #region 静态变量

        /// <summary>
        /// Error Code 子码固定长度为8
        /// </summary>
        public const int SubCodeFitLength = 8;

        /// <summary>
        /// 默认Error Code填充字符
        /// </summary>
        public const string SubCodeFitStr = "0";

        #region 自定义内部错误码

        public const string LicenseNotFind = "LIC_1001";  // 内部错误，找不到License
        public const string LicenseExpired = "LIC_1002";  // 内部错误，License过期("License expired.")
        public const string LicenseBadFormat = "LIC_1003";  // 内部错误，无效License格式("Invalid license data format.")
        public const string LicenseInvalid = "LIC_1008";  // 内部错误，无效License("Cannot find license file.")

        public const string SystemInitializedRepeatedly = "SYS_1008";  // 重复初始化("System has been initialized, cannot be initialized repeatedly.")

        public const string DataKeyRepeated = "DAT_1010";    // 重复编号

        #endregion

        #endregion

        #region 枚举

        public enum Category
        {
            Inner,      // INN
            General,    // GEN
            Message,    // MSG
            Core,       // COR
            System,     // SYS
            License,    // LIC
            Data,       // DAT
            Security,   // SEC
            Configuration,  // CFG
            Cache,      // CAH
            Communication,  // COM
            Web         // WEB
        }

        /// <summary>
        /// 获取错误编码前缀
        /// </summary>
        /// <param name="ca"></param>
        /// <returns></returns>
        internal static string GetErrorCodePrefix(Category category)
        {
            string prefix = "GEN";

            switch (category)
            {
                case Category.Message:
                    prefix = "MSG";
                    break;
                case Category.Core:
                    prefix = "COR";
                    break;
                case Category.Inner:
                    prefix = "INN";
                    break;
                case Category.System:
                    prefix = "SYS";
                    break;
                case Category.License:
                    prefix = "LIC";
                    break;
                case Category.Data:
                    prefix = "DAT";
                    break;
                case Category.Security:
                    prefix = "SEC";
                    break;
                case Category.Configuration:
                    prefix = "CFG";
                    break;
                case Category.Cache:
                    prefix = "CAH";
                    break;
                case Category.Communication:
                    prefix = "COM";
                    break;
                case Category.Web:
                    prefix = "WEB";
                    break;
                default:
                    prefix = "GEN";
                    break;
            }

            return prefix;
        }

        #endregion

        #region 属性

        public virtual string Code
        {
            get;
            protected set;
        }

        public virtual Category Categroy
        {
            get;
            protected set;
        }

        public virtual string Description
        {
            get;
            protected set;
        }

        #endregion

        #region 构造函数

        internal ErrorCode()
        {
        }

        #endregion

        #region 静态函数

        public static string GetSubCode(object code)
        {
            return code.ToString().Fill(ErrorCode.SubCodeFitStr, ErrorCode.SubCodeFitLength);
        }

        internal static ErrorCode GetInner(string innerErrCode)
        {
            return new ErrorCode()
            {
                Categroy = Category.Inner,
                Code = innerErrCode
            };
        }

        public static ErrorCode Get(Category category, object subcode, string description = "")
        {
            string scode = GetSubCode(subcode);

            return Get(category, scode, description);
        }

        public static ErrorCode Get(Category category, string subcode = "10000001", string description = "")
        {
            if (subcode.Length != ErrorCode.SubCodeFitLength)
            {
                throw new PISException("Error subcode be eight chars.");
            }

            ErrorCode errCode = new ErrorCode
            {
                Code = subcode,
                Categroy = category,
                Description = description
            };

            return errCode;
        }

        #endregion
    }
}
