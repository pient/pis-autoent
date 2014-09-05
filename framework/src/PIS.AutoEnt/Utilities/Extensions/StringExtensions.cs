using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PIS.AutoEnt
{
    public static class StringExtensions
    {
        #region 类型操作

        public static bool IsGuid(this string str)
        {
            Guid guid = Guid.Empty;

            return Guid.TryParse(str, out guid);
        }

        public static Guid ToGuid(this string str, Guid defValue)
        {
            Guid guid = Guid.Empty;

            if (!Guid.TryParse(str, out guid))
            {
                return defValue;
            }

            return guid;
        }

        public static Guid? ToGuid(this string str)
        {
            Guid guid = Guid.Empty;

            if (!Guid.TryParse(str, out guid))
            {
                return null;
            }

            return guid;
        }

        public static int? ToInteger(this string str)
        {
            int val = 0;

            if (!int.TryParse(str, out val))
            {
                return null;
            }

            return val;
        }

        public static int ToInteger(this string str, int defValue)
        {
            int val = 0;

            if (!int.TryParse(str, out val))
            {
                return defValue;
            }

            return val;
        }

        public static bool ToBoolean(this string str, bool defValue)
        {
            bool? val = str.ToBoolean();

            if (val == null)
            {
                return defValue;
            }

            return val.Value;
        }

        public static bool? ToBoolean(this string str)
        {
            if (String.Compare(str, "true", true) == 0 || String.Compare(str, "1") == 0)
            {
                return true;
            }
            else if (String.Compare(str, "false", true) == 0 || String.Compare(str, "0") == 0)
            {
                return false;
            }

            return null;
        }

        #endregion

        #region 集合操作

        /// <summary>
        /// 返回集合字符串 默认逗号分割
        /// </summary>
        /// <param name="objenum"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable objenum)
        {
            return objenum.Join(',');
        }

        /// <summary>
        /// 返回集合字符串
        /// </summary>
        /// <param name="objenum"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable objenum, char divChar)
        {
            string rtn = String.Empty;

            foreach (object tobj in objenum)
            {
                if (tobj != null && !String.IsNullOrEmpty(tobj.ToString().Trim()))
                {
                    rtn += String.Format("{0}{1}", tobj.ToString(), divChar);
                }
            }

            rtn = rtn.TrimEnd(divChar);

            return rtn;
        }

        /// <summary>
        /// 分割并包含数据
        /// </summary>
        /// <param name="objenum"></param>
        /// <returns></returns>
        public static string JoinAndQuota(this IEnumerable objenum)
        {
            return JoinAndQuota(objenum, ',');
        }

        /// <summary>
        /// 分割并包含数据
        /// </summary>
        /// <param name="objenum"></param>
        /// <param name="leftquota"></param>
        /// <param name="rightquota"></param>
        /// <returns></returns>
        public static string JoinAndQuota(this IEnumerable objenum, string leftquota, string rightquota)
        {
            return JoinAndQuota(objenum, ',', leftquota, rightquota);
        }

        /// <summary>
        /// 分割并包含数据
        /// </summary>
        /// <param name="objenum"></param>
        /// <param name="divChar"></param>
        /// <returns></returns>
        public static string JoinAndQuota(this IEnumerable objenum, char divChar)
        {
            return JoinAndQuota(objenum, divChar, "\"", "\"");
        }

        /// <summary>
        /// 分割并包含数据
        /// </summary>
        /// <param name="objenum"></param>
        /// <param name="divChar"></param>
        /// <param name="leftquota"></param>
        /// <param name="rightquota"></param>
        /// <returns></returns>
        public static string JoinAndQuota(this IEnumerable objenum, char divChar, string leftquota, string rightquota)
        {
            string rtn = String.Empty;

            foreach (object tobj in objenum)
            {
                if (tobj != null && !String.IsNullOrEmpty(tobj.ToString().Trim()))
                {
                    rtn += String.Format("{0}{1}{2}{3}", leftquota, tobj.ToString(), rightquota, divChar);
                }
            }

            rtn = rtn.TrimEnd(divChar);
            return rtn;
        }

        #endregion

        #region 模版操作

        /// <summary>
        /// 截断两侧
        /// </summary>
        /// <param name="str"></param>
        /// <param name="expstr"></param>
        /// <returns></returns>
        public static string Trim(this string str, string expstr)
        {
            return str.TrimStart(expstr).TrimEnd(expstr);
        }

        /// <summary>
        /// 截断最后
        /// </summary>
        /// <returns></returns>
        public static string TrimEnd(this string str, string expstr)
        {
            int index = str.LastIndexOf(expstr);

            if (index == str.Length - expstr.Length)
            {
                return str.Substring(0, index);
            }

            return str;
        }

        /// <summary>
        /// 截断开始
        /// </summary>
        /// <returns></returns>
        public static string TrimStart(this string str, string expstr)
        {
            int index = str.IndexOf(expstr);

            if (index == 0)
            {
                return str.Substring(expstr.Length, str.Length - expstr.Length);
            }

            return str;
        }

        /// <summary>
        /// 剥去模版外套
        /// </summary>
        /// <param name="tmplstr"></param>
        /// <param name="beginWrapper"></param>
        /// <param name="endWrapper"></param>
        /// <returns></returns>
        public static string PeerOff(this string tmplstr, string beginWrapper, string endWrapper)
        {
            string rtnstr = tmplstr;

            if (tmplstr != null)
            {
                rtnstr = tmplstr.TrimStart(beginWrapper).TrimEnd(endWrapper);
            }

            return rtnstr;
        }

        /// <summary>
        /// 包装模版字符
        /// </summary>
        /// <param name="tmplstr"></param>
        /// <param name="beginWrapper"></param>
        /// <param name="endWrapper"></param>
        /// <returns></returns>
        public static string Wrap(this string tmplstr, string beginWrapper, string endWrapper)
        {
            return String.Format("{0}{1}{2}", beginWrapper, tmplstr, endWrapper);
        }

        /// <summary>
        /// 循环由给定的字符获取指定长度的字符
        /// </summary>
        /// <param name="length"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Repeat(this string str, int length)
        {
            StringBuilder sb = new StringBuilder(length);

            char[] strchars = str.ToCharArray();

            for (int i = 0, j = 0; i < length; i++, j++)
            {
                if (j >= strchars.Length)
                {
                    j = 0;
                }

                sb.Append(strchars[j]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 用fillstr循环添加到字符串的前部或后部直到字符串长度为length
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fillstr"></param>
        /// <param name="length"></param>
        /// <param name="isprefix"></param>
        /// <returns></returns>
        public static string Fill(this string str, string fillstr, int length, bool isprefix = true)
        {
            int fillLength = length - str.Length;

            if (fillLength <= 0)
            {
                return str;
            }

            string fstr = Repeat(fillstr, fillLength);
            string rtnStr = str;

            if (isprefix == true)
            {
                rtnStr = fstr + str;
            }
            else
            {
                rtnStr = str + fstr;
            }

            return rtnStr;
        }

        #endregion

        #region Base64 操作

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(this string str)
        {
            return str.EncodeBase64(Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(this string str, Encoding encode)
        {
            string encodestr = String.Empty;
            byte[] bytes = encode.GetBytes(str);

            try
            {
                encodestr = Convert.ToBase64String(bytes);
            }
            catch(Exception)
            {
                encodestr = str;
            }

            return encodestr;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(this string str)
        {
            return str.DecodeBase64(Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(this string str, Encoding encode)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(str);

            try
            {
                decode = encode.GetString(bytes);
            }
            catch (Exception)
            {
                decode = str;
            }

            return decode;
        }

        #endregion
    }
}
