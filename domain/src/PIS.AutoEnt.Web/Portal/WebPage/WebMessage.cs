using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PIS.AutoEnt.Web
{
    /// <summary>
    /// Web消息，用于程序与客户端交互
    /// </summary>
    public class WebMessage : Message
    {
        #region 属性

        /// <summary>
        /// 默认消息标签
        /// </summary>
        public const string DefaultMessageLabel = "__MESSAGE";

        #endregion

        #region 构造函数

        public WebMessage()
        {
            this.lable = WebMessage.DefaultMessageLabel;
        }

        public WebMessage(string msg)
            : this()
        {
            this.Content = msg;
        }

        public WebMessage(string title, string msg)
            : this(msg)
        {
            this.Content = msg;
        }

        #endregion

        #region Message 成员

        public virtual string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 消息标签
        /// </summary>
        public override string Lable
        {
            get
            {
                return base.Lable;
            }
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public override string Content
        {
            get
            {
                return base.Content;
            }
            set
            {
                base.Content = value;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 转换为Json String
        /// </summary>
        /// <returns></returns>
        public virtual string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion
    }
}
