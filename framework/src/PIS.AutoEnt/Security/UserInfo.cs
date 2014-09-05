using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [DataContract]
    public abstract class UserInfo
    {
        #region 成员

        EasyDictionary<string> _tag;

        #endregion

        #region 构造函数

        public UserInfo()
        {
        }

        #endregion

        #region 属性

        /// <summary>
        /// 登录SessionID
        /// </summary>
        [DataMember]
        public virtual string SessionId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public virtual string UserId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public virtual string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// 登录名
        /// </summary>
        [DataMember]
        public virtual string LoginName
        {
            get;
            protected set;
        }

        /// <summary>
        /// 安全级别
        /// </summary>
        [DataMember]
        public virtual int SecurityLevel
        {
            get;
            protected set;
        }

        /// <summary>
        /// 用户语言
        /// </summary>
        public virtual string Language
        {
            get;
            protected set;
        }

        /// <summary>
        /// 最后访问系统时间
        /// </summary>
        [DataMember]
        public virtual DateTime LastAccessed
        {
            get;
            protected set;
        }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public EasyDictionary<string> ExtData
        {
            get
            {
                if (_tag == null)
                {
                    _tag = new EasyDictionary<string>();
                }

                return _tag;
            }
        }

        #endregion

        #region Static Methods

        public void SetLastAccessed(DateTime? dateTime = null)
        {
            if (dateTime == null)
            {
                dateTime = DateTime.UtcNow;
            }

            this.LastAccessed = dateTime.Value;
        }

        #endregion
    }
}
