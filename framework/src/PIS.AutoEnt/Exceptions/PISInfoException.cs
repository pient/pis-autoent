using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 消息异常（用于传递消息，一般并不作特殊处理或写入日志）
    /// </summary>
    public class PISInfoException : PISException
    {
        #region 成员属性

        private Message _Message = null;

        /// <summary>
        /// 消息体
        /// </summary>
        new public Message Message
        {
            get
            {
                if (_Message == null)
                {
                    _Message = new Message();
                }

                return _Message;
            }
            set
            {
                Message = value;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public PISInfoException()
        {
        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="message">异常的消息</param>
        public PISInfoException(string message)
            : base(message)
        {
            Message.Content = message;
        }

        public PISInfoException(string message, Exception innerException)
            : base(message, innerException)
        {
            Message.Content = message;
        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="info">存有有关所引发异常的序列化的对象数据</param>
        /// <param name="context">包含有关源或目标的上下文信息</param>
        protected PISInfoException(Message message)
            : base(message.Content)
        {
            Message = message;
        }

        public PISInfoException(Message message, Exception innerException)
            : base(message.Content, innerException)
        {
            Message = message;
        }

        #endregion
    }
}
