using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace PIS.AutoEnt.Web
{
    /// <summary>
    /// 页面状态容器控件
    /// </summary>
    internal class PageStateContainer : System.Web.UI.WebControls.WebControl
    {
        #region 私有函数

        private string _value = String.Empty;

        #endregion

        #region 属性

        /// <summary>
        /// 控件值
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region 构造函数

        public PageStateContainer()
        {
        }

        #endregion

        #region 重载

        /// <summary>
        /// 渲染内容
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.Write(String.Format("<input type='hidden' name='__PAGESTATE' id='__PAGESTATE' value='{0}' />", FormatValue(this._value)));

            writer.RenderEndTag();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 格式化字符串值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string FormatValue(string val)
        {
            string rtnstr = val;

            if (!String.IsNullOrEmpty(val))
            {
                rtnstr = HttpUtility.HtmlEncode(val);

                // 替换单引号双引号
                return rtnstr.Replace("'", "&#39;").Replace("\"", "&#34;");
            }
            else
            {
                return val;
            }
        }

        #endregion
    }
}
