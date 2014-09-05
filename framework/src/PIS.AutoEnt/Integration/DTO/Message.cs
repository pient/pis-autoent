using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        Data,    // 数据消息（默认）
        Operation,  // 操作消息（命令）
        Exception,  // 异常消息
        Callback,   // 返回消息
        Other
    }

    /// <summary>
    /// 消息格式(Text 默认, Json, Xml)
    /// </summary>
    public enum MessageFormat
    {
        Text,
        Json,
        Xml
    }

    /// <summary>
    /// 沿用NServiceBus的IMessage, 考虑与NServiceBus集成
    /// </summary>
    public interface IMessage
    {
        MessageFormat Format { get; }
    }

    public interface IServiceMessage : IMessage
    {
        MessageType MessageType { get; }
    }

    /// <summary>
    /// 消息基类，可以扩展为跟多特殊消息类
    /// </summary>
    [Serializable]
    public class Message<T> : IMessage, IServiceMessage
    {
        #region 成员属性

        /// <summary>
        /// 消息类型
        /// </summary>
        public virtual MessageType MessageType
        {
            get
            {
                return MessageType.Data;
            }
        }

        public MessageFormat Format
        {
            get { return MessageFormat.Text; }
        }

        protected string lable = String.Empty;

        /// <summary>
        /// 消息标签
        /// </summary>
        public virtual string Lable
        {
            get { return lable; }
            set { lable = value; }
        }

        private T content;

        /// <summary>
        /// 消息内容
        /// </summary>
        public virtual T Content
        {
            get { return content; }
            set { content = value; }
        }

        #endregion

        #region 构造函数

        public Message()
        {
        }

        public Message(string lbl)
        {
            this.lable = lbl;
        }

        #endregion
    }

    /// <summary>
    /// 默认消息内容为字符串
    /// </summary>
    public class Message : Message<string>
    {
    }
}
