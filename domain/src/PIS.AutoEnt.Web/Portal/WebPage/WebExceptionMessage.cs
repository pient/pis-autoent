using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Web
{
    /// <summary>
    /// Web异常信息
    /// </summary>
    public class WebExceptionMessage : WebMessage
    {
        #region 静态成员

        /// <summary>
        /// 默认消息标签
        /// </summary>
        new public const string DefaultMessageLabel = "__EXCEPTION";

        /// <summary>
        /// 安全异常标签
        /// </summary>
        public const string SecurityMessageLabel = "__SEXCEPTION";

        #endregion


        #region 构造函数

        public WebExceptionMessage()
        {
            this.lable = WebExceptionMessage.DefaultMessageLabel;
        }

        public WebExceptionMessage(string exstr)
            : this()
        {
            this.Content = exstr;
        }

        public WebExceptionMessage(Exception ex)
            : this()
        {
            this.Content = ex.Message;
        }

        #endregion
    }
}
