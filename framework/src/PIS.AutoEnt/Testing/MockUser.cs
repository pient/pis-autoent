using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Testing
{
    /// <summary>
    /// 虚拟用户，测试时使用，禁止用在产品场合
    /// </summary>
    [Serializable]
    public class MockUser : UserInfo
    {
        #region 构造函数

        public MockUser(string userId, string name)
        {
            this.UserId = userId;
            this.Name = name;
        }

        public MockUser(string sessionId, string userId, string name, string loginName, int securityLevel, DateTime lastAccessed)
        {
            // 从WebService获取用户信息
            this.UserId = userId;
            this.Name = name;
            this.LoginName = loginName;
            this.SecurityLevel = securityLevel;    // max 100
            this.LastAccessed = lastAccessed;
        }

        #endregion
    }
}
